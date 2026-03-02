using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace DesignKitRecolorCopy;

internal static class NativeMethods
{
    [DllImport("user32.dll")]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll")]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll")]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
    public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
    public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll")]
    public static extern bool UpdateWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

    [DllImport("user32.dll")]
    public static extern IntPtr SetFocus(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
    public static extern IntPtr GetWindowLongPtrEx(IntPtr hWnd, int nIndex);

    public const int GWL_EXSTYLE = -20;
    public const int WS_EX_TOOLWINDOW = 0x00000080;
    public const int WS_EX_APPWINDOW = 0x00040000;
}

public partial class MainForm : Form
{
    private const string RecolorUrl = "https://www.designkit.cn/recolor/?from=tools";
    private const string WenXiaoBaiUrl = "https://www.wenxiaobai.com/chat/200006";
    private const string DouBaoUrl = "https://www.doubao.com/chat/?channel=zhisouaidaohangzhan&source=doutui_tj_db_zsaidhz_nr4e65";
    private const string HuiWaUrl = "https://www.ihuiwa.com/workspace/ai-image/flat-lay";
    private const string LinkFoxUrl = "https://ai.linkfox.com/draw/ai-dressing";

    private int _currentTool; // 0=美图换色, 1=PS(桌面), 2=问小白, 3=豆包, 4=绘蛙, 5=LINKFOX
    private WebView2? _wenXiaoBaiWebView;
    private WebView2? _douBaoWebView;
    private WebView2? _huiWaWebView;
    private WebView2? _linkFoxWebView;

    private const int GWL_STYLE = -16;
    private const int WS_CHILD = 0x40000000;
    private const int WS_POPUP = unchecked((int)0x80000000);
    private const int WS_CAPTION = 0x00C00000;
    private const int WS_THICKFRAME = 0x00040000;
    private const int WS_MINIMIZEBOX = 0x00020000;
    private const int WS_MAXIMIZEBOX = 0x00010000;
    private const int WS_SYSMENU = 0x00080000;
    private const int WS_BORDER = 0x00800000;
    private const int WS_DLGFRAME = 0x00400000;
    private const int WS_VISIBLE = 0x10000000;
    private const int WS_CLIPCHILDREN = 0x02000000;
    private const int WS_CLIPSIBLINGS = 0x04000000;

    private const uint GW_OWNER = 4;
    private const int SW_SHOW = 5;
    private const int SW_SHOWNA = 8;

    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_NOACTIVATE = 0x0010;
    private const uint SWP_FRAMECHANGED = 0x0020;
    private const uint SWP_SHOWWINDOW = 0x0040;
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;

    private static readonly IntPtr HWND_TOP = new IntPtr(0);

    /// <summary>
    /// 在页面中查找换色结果图（Base64）并 postMessage 到宿主。
    /// 优先：img[src^="data:image/png;base64"]，其次带 ai-cp-image 的容器内 img。
    /// </summary>
    private static readonly string GetRecolorImageScript = """
        (function(){
            if (typeof window.chrome === 'undefined' || !window.chrome.webview) {
                window.chrome.webview.postMessage(JSON.stringify({ error: '当前环境不支持' }));
                return;
            }
            var imgs = document.querySelectorAll('img[src^="data:image/png;base64"]');
            if (imgs.length === 0) {
                var containers = document.querySelectorAll('[class*="ai-cp-image"]');
                for (var c = 0; c < containers.length; c++) {
                    var inner = containers[c].querySelectorAll('img[src^="data:image"]');
                    for (var i = 0; i < inner.length; i++) imgs = [].concat(Array.prototype.slice.call(imgs), Array.prototype.slice.call(inner));
                }
            }
            if (!imgs || imgs.length === 0) {
                window.chrome.webview.postMessage(JSON.stringify({ error: '未找到换色结果图，请先上传图片并选择换色区域' }));
                return;
            }
            var best = imgs[0];
            var bestArea = (best.naturalWidth || best.width) * (best.naturalHeight || best.height);
            for (var i = 1; i < imgs.length; i++) {
                var area = (imgs[i].naturalWidth || imgs[i].width) * (imgs[i].naturalHeight || imgs[i].height);
                if (area > bestArea) { best = imgs[i]; bestArea = area; }
            }
            window.chrome.webview.postMessage(JSON.stringify({ src: best.src }));
        })();
        """;

    public MainForm()
    {
        InitializeComponent();
        Load += MainForm_Load;
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        try
        {
            // 使用本地应用数据目录，避免安装到 Program Files 等只读位置时 WebView2 无法写入导致白屏
            var userDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DesignKitRecolorCopy", "WebView2");
            webView.CreationProperties = new CoreWebView2CreationProperties { UserDataFolder = userDataFolder };

            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            webView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            _currentTool = 0;
            UpdateToolButtonStyle();
            btnCopyLink.Visible = true;
            btnSendToPs.Visible = true;
            lblStatus.Text = "正在加载页面…";
            webView.Source = new Uri(RecolorUrl);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "";
            var msg = ex.Message;
            var hint = msg.Contains("Dll", StringComparison.OrdinalIgnoreCase) || msg.Contains("not found", StringComparison.OrdinalIgnoreCase)
                ? "\n\n若已安装 WebView2 仍报此错：请改用「多文件」发布，把整个 publish 文件夹发给用户后再运行；或安装 WebView2「常青独立安装程序」x64 版本。"
                : "\n\n请从 Microsoft 官网下载 WebView2「常青引导程序」或「常青独立安装程序 x64」安装。";
            MessageBox.Show(
                "初始化浏览器失败，请确保已安装 WebView2 运行时。" + hint + "\n\n" + msg,
                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task InitializeWenXiaoBaiWebView()
    {
        try
        {
            if (_wenXiaoBaiWebView == null)
            {
                _wenXiaoBaiWebView = new WebView2
                {
                    Dock = DockStyle.Fill,
                    DefaultBackgroundColor = Color.White
                };
                panelWenXiaoBai.Controls.Add(_wenXiaoBaiWebView);
            }

            var userDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DesignKitRecolorCopy", "WebView2WenXiaoBai");
            _wenXiaoBaiWebView.CreationProperties = new CoreWebView2CreationProperties { UserDataFolder = userDataFolder };

            lblStatus.Text = "正在加载问小白…";
            await _wenXiaoBaiWebView.EnsureCoreWebView2Async(null);
            _wenXiaoBaiWebView.CoreWebView2.NavigationCompleted += WenXiaoBai_NavigationCompleted;
            _wenXiaoBaiWebView.Source = new Uri(WenXiaoBaiUrl);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "问小白加载失败";
            MessageBox.Show(
                "初始化问小白失败：" + ex.Message,
                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void WenXiaoBai_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        this.Invoke(() =>
        {
            if (e.IsSuccess && _currentTool == 2)
                lblStatus.Text = "问小白 - 根据工单需求生成提示词";
            else
            {
                lblStatus.Text = "问小白页面加载失败";
                MessageBox.Show(
                    "问小白页面加载失败，请检查网络连接。",
                    "加载失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        });
    }

    private async Task InitializeDouBaoWebView()
    {
        try
        {
            if (_douBaoWebView == null)
            {
                _douBaoWebView = new WebView2
                {
                    Dock = DockStyle.Fill,
                    DefaultBackgroundColor = Color.White
                };
                panelDouBao.Controls.Add(_douBaoWebView);
            }

            var userDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DesignKitRecolorCopy", "WebView2DouBao");
            _douBaoWebView.CreationProperties = new CoreWebView2CreationProperties { UserDataFolder = userDataFolder };

            lblStatus.Text = "正在加载豆包…";
            await _douBaoWebView.EnsureCoreWebView2Async(null);
            _douBaoWebView.CoreWebView2.NavigationCompleted += DouBao_NavigationCompleted;
            _douBaoWebView.Source = new Uri(DouBaoUrl);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "豆包加载失败";
            MessageBox.Show(
                "初始化豆包失败：" + ex.Message,
                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DouBao_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        this.Invoke(() =>
        {
            if (e.IsSuccess && _currentTool == 3)
                lblStatus.Text = "豆包 - AI 智能助手";
            else
            {
                lblStatus.Text = "豆包页面加载失败";
                MessageBox.Show(
                    "豆包页面加载失败，请检查网络连接。",
                    "加载失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        });
    }

    private async Task InitializeHuiWaWebView()
    {
        try
        {
            if (_huiWaWebView == null)
            {
                _huiWaWebView = new WebView2
                {
                    Dock = DockStyle.Fill,
                    DefaultBackgroundColor = Color.White
                };
                panelHuiWa.Controls.Add(_huiWaWebView);
            }

            var userDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DesignKitRecolorCopy", "WebView2HuiWa");
            _huiWaWebView.CreationProperties = new CoreWebView2CreationProperties { UserDataFolder = userDataFolder };

            lblStatus.Text = "正在加载绘蛙…";
            await _huiWaWebView.EnsureCoreWebView2Async(null);
            _huiWaWebView.CoreWebView2.NavigationCompleted += HuiWa_NavigationCompleted;
            _huiWaWebView.Source = new Uri(HuiWaUrl);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "绘蛙加载失败";
            MessageBox.Show(
                "初始化绘蛙失败：" + ex.Message,
                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void HuiWa_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        this.Invoke(() =>
        {
            if (e.IsSuccess && _currentTool == 4)
                lblStatus.Text = "绘蛙 - AI 换衣";
            else
            {
                lblStatus.Text = "绘蛙页面加载失败";
                MessageBox.Show(
                    "绘蛙页面加载失败，请检查网络连接。",
                    "加载失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        });
    }

    private async Task InitializeLinkFoxWebView()
    {
        try
        {
            if (_linkFoxWebView == null)
            {
                _linkFoxWebView = new WebView2
                {
                    Dock = DockStyle.Fill,
                    DefaultBackgroundColor = Color.White
                };
                panelLinkFox.Controls.Add(_linkFoxWebView);
            }

            var userDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DesignKitRecolorCopy", "WebView2LinkFox");
            _linkFoxWebView.CreationProperties = new CoreWebView2CreationProperties { UserDataFolder = userDataFolder };

            lblStatus.Text = "正在加载 LINKFOX…";
            await _linkFoxWebView.EnsureCoreWebView2Async(null);
            _linkFoxWebView.CoreWebView2.NavigationCompleted += LinkFox_NavigationCompleted;
            _linkFoxWebView.Source = new Uri(LinkFoxUrl);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "LINKFOX 加载失败";
            MessageBox.Show(
                "初始化 LINKFOX 失败：" + ex.Message,
                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LinkFox_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        this.Invoke(() =>
        {
            if (e.IsSuccess && _currentTool == 5)
                lblStatus.Text = "LINKFOX - AI 换衣";
            else
            {
                lblStatus.Text = "LINKFOX 页面加载失败";
                MessageBox.Show(
                    "LINKFOX 页面加载失败，请检查网络连接。",
                    "加载失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        });
    }

    private void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        this.Invoke(() =>
        {
            if (e.IsSuccess && _currentTool == 0)
                lblStatus.Text = "请在下方完成上传图片并选择换色区域后，点击「一键复制换色图」。";
            else
            {
                lblStatus.Text = "页面加载失败，请检查网络或 WebView2 运行时。";
                MessageBox.Show(
                    "页面加载失败，下方网页区域为空白时请检查：\n\n" +
                    "1. 网络是否正常\n" +
                    "2. 是否已安装 WebView2 运行时（Microsoft 官网下载「常青引导程序」安装）\n\n" +
                    "错误信息：" + (e.WebErrorStatus != CoreWebView2WebErrorStatus.Unknown ? e.WebErrorStatus.ToString() : "未知"),
                    "加载失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        });
    }

    private async void SwitchTool(int toolIndex)
    {
        if (_currentTool == toolIndex) return;
        _currentTool = toolIndex;
        UpdateToolButtonStyle();
        btnCopyLink.Visible = (_currentTool == 0);
        btnSendToPs.Visible = (_currentTool == 0);
        if (_currentTool == 0)
        {
            panelPsDesktop.Visible = false;
            panelWenXiaoBai.Visible = false;
            panelDouBao.Visible = false;
            panelHuiWa.Visible = false;
            panelLinkFox.Visible = false;
            webView.Visible = true;
            lblStatus.Text = "请在下方完成上传图片并选择换色区域后，点击「一键复制换色图」。";
        }
        else if (_currentTool == 1)
        {
            webView.Visible = false;
            panelWenXiaoBai.Visible = false;
            panelDouBao.Visible = false;
            panelHuiWa.Visible = false;
            panelLinkFox.Visible = false;
            panelPsDesktop.Visible = true;
            panelPsDesktop.BringToFront();
            LoadPsConfigToUi();
            lblStatus.Text = "点击启动后，工具会在左侧，Photoshop 在右侧分屏显示。";
        }
        else if (_currentTool == 2)
        {
            webView.Visible = false;
            panelPsDesktop.Visible = false;
            panelDouBao.Visible = false;
            panelHuiWa.Visible = false;
            panelLinkFox.Visible = false;
            panelWenXiaoBai.Visible = true;
            panelWenXiaoBai.BringToFront();
            lblStatus.Text = "问小白 - 根据工单需求生成提示词";
            
            // 初始化问小白 WebView2（如果尚未初始化）
            if (_wenXiaoBaiWebView == null || _wenXiaoBaiWebView.CoreWebView2 == null)
            {
                await InitializeWenXiaoBaiWebView();
            }
        }
        else if (_currentTool == 3)
        {
            webView.Visible = false;
            panelPsDesktop.Visible = false;
            panelWenXiaoBai.Visible = false;
            panelHuiWa.Visible = false;
            panelLinkFox.Visible = false;
            panelDouBao.Visible = true;
            panelDouBao.BringToFront();
            lblStatus.Text = "豆包 - AI 智能助手";
            
            // 初始化豆包 WebView2（如果尚未初始化）
            if (_douBaoWebView == null || _douBaoWebView.CoreWebView2 == null)
            {
                await InitializeDouBaoWebView();
            }
        }
        else if (_currentTool == 4)
        {
            webView.Visible = false;
            panelPsDesktop.Visible = false;
            panelWenXiaoBai.Visible = false;
            panelDouBao.Visible = false;
            panelLinkFox.Visible = false;
            panelHuiWa.Visible = true;
            panelHuiWa.BringToFront();
            lblStatus.Text = "绘蛙 - AI 换衣";
            
            // 初始化绘蛙 WebView2（如果尚未初始化）
            if (_huiWaWebView == null || _huiWaWebView.CoreWebView2 == null)
            {
                await InitializeHuiWaWebView();
            }
        }
        else if (_currentTool == 5)
        {
            webView.Visible = false;
            panelPsDesktop.Visible = false;
            panelWenXiaoBai.Visible = false;
            panelDouBao.Visible = false;
            panelHuiWa.Visible = false;
            panelLinkFox.Visible = true;
            panelLinkFox.BringToFront();
            lblStatus.Text = "LINKFOX - AI 换衣";
            
            // 初始化 LINKFOX WebView2（如果尚未初始化）
            if (_linkFoxWebView == null || _linkFoxWebView.CoreWebView2 == null)
            {
                await InitializeLinkFoxWebView();
            }
        }
    }

    private void UpdateToolButtonStyle()
    {
        btnToolRecolor.ForeColor = _currentTool == 0 ? Color.FromArgb(31, 41, 55) : Color.FromArgb(107, 114, 128);
        btnToolRecolor.Font = new Font(btnToolRecolor.Font.FontFamily, 9F, _currentTool == 0 ? FontStyle.Bold : FontStyle.Regular);
        btnToolPs.ForeColor = _currentTool == 1 ? Color.FromArgb(31, 41, 55) : Color.FromArgb(107, 114, 128);
        btnToolPs.Font = new Font(btnToolPs.Font.FontFamily, 9F, _currentTool == 1 ? FontStyle.Bold : FontStyle.Regular);
        btnToolWenXiaoBai.ForeColor = _currentTool == 2 ? Color.FromArgb(31, 41, 55) : Color.FromArgb(107, 114, 128);
        btnToolWenXiaoBai.Font = new Font(btnToolWenXiaoBai.Font.FontFamily, 9F, _currentTool == 2 ? FontStyle.Bold : FontStyle.Regular);
        btnToolDouBao.ForeColor = _currentTool == 3 ? Color.FromArgb(31, 41, 55) : Color.FromArgb(107, 114, 128);
        btnToolDouBao.Font = new Font(btnToolDouBao.Font.FontFamily, 9F, _currentTool == 3 ? FontStyle.Bold : FontStyle.Regular);
        btnToolHuiWa.ForeColor = _currentTool == 4 ? Color.FromArgb(31, 41, 55) : Color.FromArgb(107, 114, 128);
        btnToolHuiWa.Font = new Font(btnToolHuiWa.Font.FontFamily, 9F, _currentTool == 4 ? FontStyle.Bold : FontStyle.Regular);
        btnToolLinkFox.ForeColor = _currentTool == 5 ? Color.FromArgb(31, 41, 55) : Color.FromArgb(107, 114, 128);
        btnToolLinkFox.Font = new Font(btnToolLinkFox.Font.FontFamily, 9F, _currentTool == 5 ? FontStyle.Bold : FontStyle.Regular);
    }

    private void BtnToolRecolor_Click(object? sender, EventArgs e) => SwitchTool(0);

    private void BtnToolPs_Click(object? sender, EventArgs e) => SwitchTool(1);

    private void BtnToolWenXiaoBai_Click(object? sender, EventArgs e) => SwitchTool(2);

    private void BtnToolDouBao_Click(object? sender, EventArgs e) => SwitchTool(3);

    private void BtnToolHuiWa_Click(object? sender, EventArgs e) => SwitchTool(4);

    private void BtnToolLinkFox_Click(object? sender, EventArgs e) => SwitchTool(5);

    private void BtnLaunchPs_Click(object? sender, EventArgs e)
    {
        var psPath = GetPhotoshopPath();
        if (string.IsNullOrEmpty(psPath))
        {
            MessageBox.Show(
                "未找到 Photoshop。\n\n请在「设置」中配置 Photoshop.exe 的安装路径。",
                "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        
        // 启动 PS 并自动分屏
        LaunchPsWithSideBySide(psPath);
    }

    /// <summary>
    /// 启动 PS 并自动排列窗口（工具在左，PS 在右）。
    /// </summary>
    private void LaunchPsWithSideBySide(string psPath)
    {
        try
        {
            // 启动 PS
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = psPath,
                    UseShellExecute = true
                }
            };
            process.Start();
            
            lblStatus.Text = "正在启动 Photoshop 并自动排列窗口…";
            
            // 等待 PS 窗口出现后调整布局
            System.Threading.Tasks.Task.Run(async () =>
            {
                for (int i = 0; i < 30; i++)
                {
                    await System.Threading.Tasks.Task.Delay(1000);
                    var psWindow = FindPhotoshopWindow();
                    if (psWindow != IntPtr.Zero)
                    {
                        this.Invoke(() => ArrangeSideBySide(psWindow));
                        return;
                    }
                }
                this.Invoke(() => lblStatus.Text = "Photoshop 已启动，但未找到窗口。请手动调整窗口位置。");
            });
        }
        catch (Exception ex)
        {
            lblStatus.Text = "启动 Photoshop 失败。";
            MessageBox.Show($"启动失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// 自动排列工具和 PS 窗口（工具在左侧占屏幕 40%，PS 在右侧占 60%）。
    /// </summary>
    private void ArrangeSideBySide(IntPtr psWindow)
    {
        try
        {
            // 获取屏幕工作区域
            var screen = Screen.FromHandle(this.Handle);
            var workingArea = screen.WorkingArea;
            
            // 计算工具窗口和 PS 窗口的位置
            int toolWidth = (int)(workingArea.Width * 0.4);
            int psWidth = workingArea.Width - toolWidth;
            
            // 调整工具窗口（左侧 40%）
            this.WindowState = FormWindowState.Normal;
            this.Location = new Point(workingArea.X, workingArea.Y);
            this.Size = new Size(toolWidth, workingArea.Height);
            
            // 调整 PS 窗口（右侧 60%）
            NativeMethods.ShowWindow(psWindow, SW_SHOW);
            NativeMethods.SetWindowPos(
                psWindow,
                HWND_TOP,
                workingArea.X + toolWidth,
                workingArea.Y,
                psWidth,
                workingArea.Height,
                SWP_SHOWWINDOW);
            
            lblStatus.Text = "已自动排列窗口：工具在左，Photoshop 在右。";
            btnRearrangeWindows.Visible = true; // 显示重新排列按钮
        }
        catch (Exception ex)
        {
            lblStatus.Text = "窗口排列失败。";
            MessageBox.Show($"窗口排列失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnRearrangeWindows_Click(object? sender, EventArgs e)
    {
        if (!IsPhotoshopRunning())
        {
            MessageBox.Show(
                "Photoshop 未运行。\n\n请先启动 Photoshop。",
                "提示",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        var psWindow = FindPhotoshopWindow();
        if (psWindow == IntPtr.Zero)
        {
            MessageBox.Show("未找到 Photoshop 窗口。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ArrangeSideBySide(psWindow);
    }


    private string? GetPhotoshopPath()
    {
        var configured = LoadPsConfigPath();
        if (!string.IsNullOrWhiteSpace(configured) && File.Exists(configured))
            return configured;
        return DetectPhotoshopPath();
    }

    internal static string GetConfigFilePath()
    {
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DesignKitRecolorCopy");
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, "settings.json");
    }

    internal static string? LoadPsConfigPath()
    {
        try
        {
            var path = GetConfigFilePath();
            if (!File.Exists(path)) return null;
            var json = File.ReadAllText(path);
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("PhotoshopPath", out var prop))
                return prop.GetString();
        }
        catch { /* ignore */ }
        return null;
    }

    internal static void SavePsConfigPath(string? psPath)
    {
        try
        {
            var obj = new Dictionary<string, string?>();
            if (!string.IsNullOrWhiteSpace(psPath)) obj["PhotoshopPath"] = psPath;
            var path = GetConfigFilePath();
            File.WriteAllText(path, JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch { /* ignore */ }
    }

    private void LoadPsConfigToUi()
    {
        var path = LoadPsConfigPath();
        lblPsPathDisplay.Text = string.IsNullOrWhiteSpace(path) ? "当前路径：未配置（请在「设置」中配置）" : "当前路径：" + path;
    }

    private void OpenSettings()
    {
        using var frm = new SettingsForm { Owner = this };
        frm.ShowDialog();
        LoadPsConfigToUi();
    }

    private void BtnSettings_Click(object? sender, EventArgs e) => OpenSettings();

    private void BtnOpenSettings_Click(object? sender, EventArgs e) => OpenSettings();

    private static string? DetectPhotoshopPath()
    {
        var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        var programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        var appDir = AppContext.BaseDirectory;

        var candidates = new List<string>
        {
            Path.Combine(appDir, "Photoshop.exe")
        };

        if (!string.IsNullOrEmpty(programFiles))
        {
            try
            {
                var adobe = Path.Combine(programFiles, "Adobe");
                if (Directory.Exists(adobe))
                    candidates.AddRange(Directory.GetDirectories(adobe, "Adobe Photoshop*").Select(d => Path.Combine(d, "Photoshop.exe")));
            }
            catch { /* ignore */ }
        }

        if (!string.IsNullOrEmpty(programFilesX86))
        {
            try
            {
                var adobe = Path.Combine(programFilesX86, "Adobe");
                if (Directory.Exists(adobe))
                    candidates.AddRange(Directory.GetDirectories(adobe, "Adobe Photoshop*").Select(d => Path.Combine(d, "Photoshop.exe")));
            }
            catch { /* ignore */ }
        }

        return candidates.FirstOrDefault(File.Exists);
    }

    private void CoreWebView2_WebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
    {
        try
        {
            var json = e.TryGetWebMessageAsString();
            if (string.IsNullOrEmpty(json)) return;

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (root.TryGetProperty("error", out var err))
            {
                this.Invoke(() => MessageBox.Show(err.GetString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information));
                return;
            }
            if (root.TryGetProperty("src", out var src))
            {
                var link = src.GetString();
                if (!string.IsNullOrEmpty(link))
                {
                    this.Invoke(() =>
                    {
                        try
                        {
                            CopyImageFromDataUrl(link);
                            lblStatus.Text = "已复制换色图到剪贴板";
                            
                            // 如果 PS 正在运行，提示是否自动粘贴
                            if (IsPhotoshopRunning())
                            {
                                var result = MessageBox.Show(
                                    "检测到 Photoshop 正在运行，是否自动粘贴图片到 PS？",
                                    "提示", 
                                    MessageBoxButtons.YesNo, 
                                    MessageBoxIcon.Question);
                                
                                if (result == DialogResult.Yes)
                                {
                                    PasteToPhotoshop();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"复制失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    });
                }
            }
        }
        catch (Exception ex)
        {
            this.Invoke(() => MessageBox.Show($"处理结果失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning));
        }
    }

    /// <summary>
    /// 从 data:image/...;base64,XXXX 格式的 URL 中解码图片并复制到剪贴板（作为图片，粘贴时得到图片而非链接文本）。
    /// </summary>
    private static void CopyImageFromDataUrl(string dataUrl)
    {
        if (string.IsNullOrEmpty(dataUrl) || !dataUrl.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("不是有效的 data URL", nameof(dataUrl));

        var comma = dataUrl.IndexOf(',');
        if (comma < 0)
            throw new ArgumentException("data URL 格式错误，缺少 base64 数据", nameof(dataUrl));

        var base64 = dataUrl[(comma + 1)..].Trim();
        var bytes = Convert.FromBase64String(base64);

        using var ms = new MemoryStream(bytes);
        using var image = Image.FromStream(ms);
        Clipboard.SetImage((Image)image.Clone());
    }

    private async void BtnCopyLink_Click(object? sender, EventArgs e)
    {
        if (webView.CoreWebView2 == null)
        {
            MessageBox.Show("页面尚未加载完成，请稍候再试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        lblStatus.Text = "正在获取换色图…";
        try
        {
            await webView.CoreWebView2.ExecuteScriptAsync(GetRecolorImageScript);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "";
            MessageBox.Show($"执行脚本失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnSendToPs_Click(object? sender, EventArgs e)
    {
        if (!Clipboard.ContainsImage())
        {
            MessageBox.Show(
                "剪贴板中没有图片。\n\n请先点击「一键复制换色图」复制图片。",
                "提示",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        if (!IsPhotoshopRunning())
        {
            MessageBox.Show(
                "Photoshop 未运行。\n\n请先启动 Photoshop。",
                "提示",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        PasteToPhotoshop();
    }


    /// <summary>
    /// 检测 Photoshop 是否正在运行。
    /// </summary>
    private static bool IsPhotoshopRunning()
    {
        try
        {
            var processes = System.Diagnostics.Process.GetProcessesByName("Photoshop");
            return processes.Length > 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 将剪贴板图片粘贴到 Photoshop。
    /// </summary>
    private static void PasteToPhotoshop()
    {
        try
        {
            // 查找 Photoshop 窗口
            var psWindow = FindPhotoshopWindow();
            if (psWindow == IntPtr.Zero)
            {
                MessageBox.Show("未找到 Photoshop 窗口。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 保存当前窗口
            var currentWindow = NativeMethods.GetForegroundWindow();

            // 激活 Photoshop 窗口
            NativeMethods.SetForegroundWindow(psWindow);
            System.Threading.Thread.Sleep(200); // 等待窗口激活

            // 发送 Ctrl+V 粘贴命令
            SendKeys.SendWait("^v");
            System.Threading.Thread.Sleep(100);

            // 恢复之前的窗口
            if (currentWindow != IntPtr.Zero)
            {
                NativeMethods.SetForegroundWindow(currentWindow);
            }

            MessageBox.Show("已将图片粘贴到 Photoshop。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"粘贴到 Photoshop 失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// 查找 Photoshop 主窗口。
    /// </summary>
    private static IntPtr FindPhotoshopWindow()
    {
        try
        {
            var processes = System.Diagnostics.Process.GetProcessesByName("Photoshop");
            foreach (var process in processes)
            {
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    return process.MainWindowHandle;
                }
            }
        }
        catch { }
        return IntPtr.Zero;
    }
}
