namespace DesignKitRecolorCopy;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelTop = new Panel();
        panelSeparator = new Panel();
        panelTools = new Panel();
        btnToolRecolor = new Button();
        btnToolPs = new Button();
        btnToolWenXiaoBai = new Button();
        btnToolDouBao = new Button();
        btnToolHuiWa = new Button();
        btnToolLinkFox = new Button();
        btnSettings = new Button();
        panelRight = new Panel();
        btnCopyLink = new Button();
        btnSendToPs = new Button();
        lblStatus = new Label();
        panelPsDesktop = new Panel();
        panelPsTop = new Panel();
        lblPsHint = new Label();
        lblPsPathDisplay = new Label();
        btnOpenSettings = new Button();
        btnLaunchPs = new Button();
        btnRearrangeWindows = new Button();
        panelWenXiaoBai = new Panel();
        panelDouBao = new Panel();
        panelHuiWa = new Panel();
        panelLinkFox = new Panel();
        webView = new Microsoft.Web.WebView2.WinForms.WebView2();
        panelTop.SuspendLayout();
        panelTools.SuspendLayout();
        panelRight.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
        SuspendLayout();
        //
        // panelTop
        //
        panelTop.BackColor = Color.White;
        panelTop.Controls.Add(panelSeparator);
        panelTop.Controls.Add(lblStatus);
        panelTop.Controls.Add(panelRight);
        panelTop.Controls.Add(panelTools);
        panelTop.Dock = DockStyle.Top;
        panelTop.Height = 60;
        panelTop.Padding = new Padding(0);
        panelTop.Name = "panelTop";
        panelTop.Size = new Size(1000, 60);
        panelTop.TabIndex = 0;
        //
        // panelSeparator
        //
        panelSeparator.BackColor = Color.FromArgb(229, 231, 235);
        panelSeparator.Dock = DockStyle.Bottom;
        panelSeparator.Height = 1;
        panelSeparator.Name = "panelSeparator";
        panelSeparator.Size = new Size(1000, 1);
        panelSeparator.TabIndex = 2;
        //
        // panelTools
        //
        panelTools.BackColor = Color.Transparent;
        panelTools.Controls.Add(btnSettings);
        panelTools.Controls.Add(btnToolLinkFox);
        panelTools.Controls.Add(btnToolHuiWa);
        panelTools.Controls.Add(btnToolDouBao);
        panelTools.Controls.Add(btnToolWenXiaoBai);
        panelTools.Controls.Add(btnToolPs);
        panelTools.Controls.Add(btnToolRecolor);
        panelTools.Dock = DockStyle.Left;
        panelTools.Name = "panelTools";
        panelTools.Padding = new Padding(12, 12, 8, 12);
        panelTools.Size = new Size(640, 60);
        panelTools.TabIndex = 3;
        //
        // btnToolRecolor
        //
        btnToolRecolor.FlatAppearance.BorderSize = 0;
        btnToolRecolor.FlatStyle = FlatStyle.Flat;
        btnToolRecolor.Font = new Font("Microsoft YaHei UI", 9F);
        btnToolRecolor.ForeColor = Color.FromArgb(55, 65, 81);
        btnToolRecolor.Location = new Point(12, 10);
        btnToolRecolor.Name = "btnToolRecolor";
        btnToolRecolor.Size = new Size(92, 36);
        btnToolRecolor.TabIndex = 0;
        btnToolRecolor.Text = "美图换色";
        btnToolRecolor.UseVisualStyleBackColor = true;
        btnToolRecolor.Click += BtnToolRecolor_Click;
        //
        // btnToolPs
        //
        btnToolPs.FlatAppearance.BorderSize = 0;
        btnToolPs.FlatStyle = FlatStyle.Flat;
        btnToolPs.Font = new Font("Microsoft YaHei UI", 9F);
        btnToolPs.ForeColor = Color.FromArgb(107, 114, 128);
        btnToolPs.Location = new Point(108, 10);
        btnToolPs.Name = "btnToolPs";
        btnToolPs.Size = new Size(100, 36);
        btnToolPs.TabIndex = 1;
        btnToolPs.Text = "PS(桌面)";
        btnToolPs.UseVisualStyleBackColor = true;
        btnToolPs.Click += BtnToolPs_Click;
        //
        // btnToolWenXiaoBai
        //
        btnToolWenXiaoBai.FlatAppearance.BorderSize = 0;
        btnToolWenXiaoBai.FlatStyle = FlatStyle.Flat;
        btnToolWenXiaoBai.Font = new Font("Microsoft YaHei UI", 9F);
        btnToolWenXiaoBai.ForeColor = Color.FromArgb(107, 114, 128);
        btnToolWenXiaoBai.Location = new Point(212, 10);
        btnToolWenXiaoBai.Name = "btnToolWenXiaoBai";
        btnToolWenXiaoBai.Size = new Size(92, 36);
        btnToolWenXiaoBai.TabIndex = 2;
        btnToolWenXiaoBai.Text = "问小白";
        btnToolWenXiaoBai.UseVisualStyleBackColor = true;
        btnToolWenXiaoBai.Click += BtnToolWenXiaoBai_Click;
        //
        // btnToolDouBao
        //
        btnToolDouBao.FlatAppearance.BorderSize = 0;
        btnToolDouBao.FlatStyle = FlatStyle.Flat;
        btnToolDouBao.Font = new Font("Microsoft YaHei UI", 9F);
        btnToolDouBao.ForeColor = Color.FromArgb(107, 114, 128);
        btnToolDouBao.Location = new Point(308, 10);
        btnToolDouBao.Name = "btnToolDouBao";
        btnToolDouBao.Size = new Size(76, 36);
        btnToolDouBao.TabIndex = 3;
        btnToolDouBao.Text = "豆包";
        btnToolDouBao.UseVisualStyleBackColor = true;
        btnToolDouBao.Click += BtnToolDouBao_Click;
        //
        // btnToolHuiWa
        //
        btnToolHuiWa.FlatAppearance.BorderSize = 0;
        btnToolHuiWa.FlatStyle = FlatStyle.Flat;
        btnToolHuiWa.Font = new Font("Microsoft YaHei UI", 9F);
        btnToolHuiWa.ForeColor = Color.FromArgb(107, 114, 128);
        btnToolHuiWa.Location = new Point(388, 10);
        btnToolHuiWa.Name = "btnToolHuiWa";
        btnToolHuiWa.Size = new Size(76, 36);
        btnToolHuiWa.TabIndex = 4;
        btnToolHuiWa.Text = "绘蛙";
        btnToolHuiWa.UseVisualStyleBackColor = true;
        btnToolHuiWa.Click += BtnToolHuiWa_Click;
        //
        // btnToolLinkFox
        //
        btnToolLinkFox.FlatAppearance.BorderSize = 0;
        btnToolLinkFox.FlatStyle = FlatStyle.Flat;
        btnToolLinkFox.Font = new Font("Microsoft YaHei UI", 9F);
        btnToolLinkFox.ForeColor = Color.FromArgb(107, 114, 128);
        btnToolLinkFox.Location = new Point(468, 10);
        btnToolLinkFox.Name = "btnToolLinkFox";
        btnToolLinkFox.Size = new Size(92, 36);
        btnToolLinkFox.TabIndex = 5;
        btnToolLinkFox.Text = "LINKFOX";
        btnToolLinkFox.UseVisualStyleBackColor = true;
        btnToolLinkFox.Click += BtnToolLinkFox_Click;
        //
        // btnSettings
        //
        btnSettings.FlatAppearance.BorderSize = 0;
        btnSettings.FlatStyle = FlatStyle.Flat;
        btnSettings.Font = new Font("Microsoft YaHei UI", 9F);
        btnSettings.ForeColor = Color.FromArgb(107, 114, 128);
        btnSettings.Location = new Point(564, 10);
        btnSettings.Name = "btnSettings";
        btnSettings.Size = new Size(64, 36);
        btnSettings.TabIndex = 6;
        btnSettings.Text = "设置";
        btnSettings.UseVisualStyleBackColor = true;
        btnSettings.Click += BtnSettings_Click;
        //
        // panelRight
        //
        panelRight.BackColor = Color.Transparent;
        panelRight.Controls.Add(btnSendToPs);
        panelRight.Controls.Add(btnCopyLink);
        panelRight.Dock = DockStyle.Right;
        panelRight.Location = new Point(172, 0);
        panelRight.Name = "panelRight";
        panelRight.Padding = new Padding(0, 0, 0, 0);
        panelRight.Size = new Size(388, 60);
        panelRight.TabIndex = 1;
        //
        // btnCopyLink
        //
        btnCopyLink.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCopyLink.BackColor = Color.FromArgb(55, 65, 81);
        btnCopyLink.Cursor = Cursors.Hand;
        btnCopyLink.FlatAppearance.BorderSize = 0;
        btnCopyLink.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 41, 55);
        btnCopyLink.FlatStyle = FlatStyle.Flat;
        btnCopyLink.Font = new Font("Microsoft YaHei UI", 9.25F, FontStyle.Regular);
        btnCopyLink.ForeColor = Color.White;
        btnCopyLink.Location = new Point(16, 12);
        btnCopyLink.Margin = new Padding(0);
        btnCopyLink.MinimumSize = new Size(0, 36);
        btnCopyLink.Name = "btnCopyLink";
        btnCopyLink.Size = new Size(180, 36);
        btnCopyLink.TabIndex = 0;
        btnCopyLink.Text = "一键复制换色图";
        btnCopyLink.UseVisualStyleBackColor = false;
        btnCopyLink.Click += BtnCopyLink_Click;
        //
        // btnSendToPs
        //
        btnSendToPs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSendToPs.BackColor = Color.FromArgb(79, 70, 229);
        btnSendToPs.Cursor = Cursors.Hand;
        btnSendToPs.FlatAppearance.BorderSize = 0;
        btnSendToPs.FlatAppearance.MouseOverBackColor = Color.FromArgb(67, 56, 202);
        btnSendToPs.FlatStyle = FlatStyle.Flat;
        btnSendToPs.Font = new Font("Microsoft YaHei UI", 9.25F, FontStyle.Regular);
        btnSendToPs.ForeColor = Color.White;
        btnSendToPs.Location = new Point(204, 12);
        btnSendToPs.Margin = new Padding(0);
        btnSendToPs.MinimumSize = new Size(0, 36);
        btnSendToPs.Name = "btnSendToPs";
        btnSendToPs.Size = new Size(172, 36);
        btnSendToPs.TabIndex = 1;
        btnSendToPs.Text = "发送到 Photoshop";
        btnSendToPs.UseVisualStyleBackColor = false;
        btnSendToPs.Click += BtnSendToPs_Click;
        //
        // lblStatus
        //
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.AutoEllipsis = true;
        lblStatus.ForeColor = Color.FromArgb(107, 114, 128);
        lblStatus.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);
        lblStatus.Location = new Point(640, 0);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(8, 0, 0, 0);
        lblStatus.Size = new Size(-468, 60);
        lblStatus.TabIndex = 0;
        lblStatus.Text = "请在下方完成上传图片并选择换色区域后，点击「一键复制换色图」。";
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;
        //
        // panelPsDesktop
        //
        panelPsDesktop.BackColor = Color.FromArgb(248, 249, 250);
        panelPsDesktop.Controls.Add(panelPsTop);
        panelPsDesktop.Dock = DockStyle.Fill;
        panelPsDesktop.Location = new Point(0, 60);
        panelPsDesktop.Name = "panelPsDesktop";
        panelPsDesktop.Size = new Size(1000, 540);
        panelPsDesktop.TabIndex = 4;
        panelPsDesktop.Visible = false;
        //
        // panelPsTop
        //
        panelPsTop.BackColor = Color.FromArgb(248, 249, 250);
        panelPsTop.Controls.Add(btnRearrangeWindows);
        panelPsTop.Controls.Add(btnLaunchPs);
        panelPsTop.Controls.Add(btnOpenSettings);
        panelPsTop.Controls.Add(lblPsPathDisplay);
        panelPsTop.Controls.Add(lblPsHint);
        panelPsTop.Dock = DockStyle.Fill;
        panelPsTop.Name = "panelPsTop";
        panelPsTop.Padding = new Padding(24, 16, 24, 12);
        panelPsTop.Size = new Size(1000, 540);
        panelPsTop.TabIndex = 0;
        //
        // lblPsHint
        //
        lblPsHint.AutoSize = true;
        lblPsHint.Font = new Font("Microsoft YaHei UI", 10F);
        lblPsHint.ForeColor = Color.FromArgb(107, 114, 128);
        lblPsHint.Location = new Point(24, 16);
        lblPsHint.Name = "lblPsHint";
        lblPsHint.Size = new Size(600, 22);
        lblPsHint.TabIndex = 0;
        lblPsHint.Text = "点击启动后，工具会自动排列在屏幕左侧，Photoshop 在右侧，方便同时操作。";
        //
        // lblPsPathDisplay
        //
        lblPsPathDisplay.AutoSize = true;
        lblPsPathDisplay.Font = new Font("Microsoft YaHei UI", 9F);
        lblPsPathDisplay.ForeColor = Color.FromArgb(107, 114, 128);
        lblPsPathDisplay.Location = new Point(24, 44);
        lblPsPathDisplay.Name = "lblPsPathDisplay";
        lblPsPathDisplay.Size = new Size(280, 17);
        lblPsPathDisplay.TabIndex = 1;
        lblPsPathDisplay.Text = "当前路径：未配置（请在「设置」中配置）";
        //
        // btnOpenSettings
        //
        btnOpenSettings.BackColor = Color.FromArgb(55, 65, 81);
        btnOpenSettings.Cursor = Cursors.Hand;
        btnOpenSettings.FlatAppearance.BorderSize = 0;
        btnOpenSettings.FlatStyle = FlatStyle.Flat;
        btnOpenSettings.Font = new Font("Microsoft YaHei UI", 9F);
        btnOpenSettings.ForeColor = Color.White;
        btnOpenSettings.Location = new Point(24, 66);
        btnOpenSettings.Name = "btnOpenSettings";
        btnOpenSettings.Size = new Size(80, 28);
        btnOpenSettings.TabIndex = 2;
        btnOpenSettings.Text = "设置…";
        btnOpenSettings.UseVisualStyleBackColor = false;
        btnOpenSettings.Click += BtnOpenSettings_Click;
        //
        // btnLaunchPs
        //
        btnLaunchPs.BackColor = Color.FromArgb(55, 65, 81);
        btnLaunchPs.Cursor = Cursors.Hand;
        btnLaunchPs.FlatAppearance.BorderSize = 0;
        btnLaunchPs.FlatStyle = FlatStyle.Flat;
        btnLaunchPs.Font = new Font("Microsoft YaHei UI", 9.25F);
        btnLaunchPs.ForeColor = Color.White;
        btnLaunchPs.Location = new Point(116, 64);
        btnLaunchPs.Name = "btnLaunchPs";
        btnLaunchPs.Size = new Size(180, 36);
        btnLaunchPs.TabIndex = 3;
        btnLaunchPs.Text = "启动并分屏显示";
        btnLaunchPs.UseVisualStyleBackColor = false;
        btnLaunchPs.Click += BtnLaunchPs_Click;
        //
        // btnRearrangeWindows
        //
        btnRearrangeWindows.BackColor = Color.FromArgb(59, 130, 246);
        btnRearrangeWindows.Cursor = Cursors.Hand;
        btnRearrangeWindows.FlatAppearance.BorderSize = 0;
        btnRearrangeWindows.FlatStyle = FlatStyle.Flat;
        btnRearrangeWindows.Font = new Font("Microsoft YaHei UI", 9.25F);
        btnRearrangeWindows.ForeColor = Color.White;
        btnRearrangeWindows.Location = new Point(304, 64);
        btnRearrangeWindows.Name = "btnRearrangeWindows";
        btnRearrangeWindows.Size = new Size(120, 36);
        btnRearrangeWindows.TabIndex = 5;
        btnRearrangeWindows.Text = "重新排列窗口";
        btnRearrangeWindows.UseVisualStyleBackColor = false;
        btnRearrangeWindows.Click += BtnRearrangeWindows_Click;
        btnRearrangeWindows.Visible = false;
        //
        // panelWenXiaoBai
        //
        panelWenXiaoBai.BackColor = Color.White;
        panelWenXiaoBai.Dock = DockStyle.Fill;
        panelWenXiaoBai.Location = new Point(0, 60);
        panelWenXiaoBai.Name = "panelWenXiaoBai";
        panelWenXiaoBai.Size = new Size(1000, 540);
        panelWenXiaoBai.TabIndex = 5;
        panelWenXiaoBai.Visible = false;
        //
        // panelDouBao
        //
        panelDouBao.BackColor = Color.White;
        panelDouBao.Dock = DockStyle.Fill;
        panelDouBao.Location = new Point(0, 60);
        panelDouBao.Name = "panelDouBao";
        panelDouBao.Size = new Size(1000, 540);
        panelDouBao.TabIndex = 6;
        panelDouBao.Visible = false;
        //
        // panelHuiWa
        //
        panelHuiWa.BackColor = Color.White;
        panelHuiWa.Dock = DockStyle.Fill;
        panelHuiWa.Location = new Point(0, 60);
        panelHuiWa.Name = "panelHuiWa";
        panelHuiWa.Size = new Size(1000, 540);
        panelHuiWa.TabIndex = 7;
        panelHuiWa.Visible = false;
        //
        // panelLinkFox
        //
        panelLinkFox.BackColor = Color.White;
        panelLinkFox.Dock = DockStyle.Fill;
        panelLinkFox.Location = new Point(0, 60);
        panelLinkFox.Name = "panelLinkFox";
        panelLinkFox.Size = new Size(1000, 540);
        panelLinkFox.TabIndex = 8;
        panelLinkFox.Visible = false;
        //
        // webView
        //
        webView.CreationProperties = null;
        webView.DefaultBackgroundColor = Color.White;
        webView.Dock = DockStyle.Fill;
        webView.Location = new Point(0, 60);
        webView.Name = "webView";
        webView.Size = new Size(1000, 540);
        webView.TabIndex = 1;
        webView.ZoomFactor = 1.0;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 600);
        Controls.Add(webView);
        Controls.Add(panelLinkFox);
        Controls.Add(panelHuiWa);
        Controls.Add(panelDouBao);
        Controls.Add(panelWenXiaoBai);
        Controls.Add(panelPsDesktop);
        Controls.Add(panelTop);
        MinimumSize = new Size(800, 500);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "设计工具集（美图换色 / PS 桌面 / 问小白 / 豆包 / 绘蛙 / LINKFOX）";
        panelTop.ResumeLayout(false);
        panelTools.ResumeLayout(false);
        panelRight.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)webView).EndInit();
        ResumeLayout(false);
    }

    private Panel panelTop;
    private Panel panelSeparator;
    private Panel panelTools;
    private Button btnToolRecolor;
    private Button btnToolPs;
    private Button btnToolWenXiaoBai;
    private Button btnToolDouBao;
    private Button btnToolHuiWa;
    private Button btnToolLinkFox;
    private Panel panelPsDesktop;
    private Panel panelPsTop;
    private Label lblPsHint;
    private Label lblPsPathDisplay;
    private Button btnOpenSettings;
    private Button btnLaunchPs;
    private Button btnRearrangeWindows;
    private Panel panelWenXiaoBai;
    private Panel panelDouBao;
    private Panel panelHuiWa;
    private Panel panelLinkFox;
    private Button btnSettings;
    private Panel panelRight;
    private Button btnCopyLink;
    private Button btnSendToPs;
    private Label lblStatus;
    private Microsoft.Web.WebView2.WinForms.WebView2 webView;
}
