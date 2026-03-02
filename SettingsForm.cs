namespace DesignKitRecolorCopy;

public sealed class SettingsForm : Form
{
    private TextBox _txtPsPath = null!;
    private Button _btnBrowse = null!;

    public SettingsForm()
    {
        Text = "设置";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(520, 200);
        MinimizeBox = false;
        MaximizeBox = false;
        ShowInTaskbar = false;
        BackColor = Color.White;
        Padding = new Padding(24);
        BuildUi();
    }

    private void BuildUi()
    {
        var lbl = new Label
        {
            Text = "Photoshop 安装路径（Photoshop.exe）",
            Font = new Font("Microsoft YaHei UI", 9F),
            ForeColor = Color.FromArgb(55, 65, 81),
            Location = new Point(24, 20),
            AutoSize = true
        };
        _txtPsPath = new TextBox
        {
            ReadOnly = true,
            Font = new Font("Microsoft YaHei UI", 9F),
            Location = new Point(24, 48),
            Size = new Size(360, 23),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };
        _btnBrowse = new Button
        {
            Text = "浏览…",
            Font = new Font("Microsoft YaHei UI", 9F),
            BackColor = Color.FromArgb(55, 65, 81),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = { BorderSize = 0 },
            Location = new Point(392, 46),
            Size = new Size(80, 28),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Cursor = Cursors.Hand
        };
        _btnBrowse.Click += BtnBrowse_Click;

        var btnOk = new Button
        {
            Text = "确定",
            DialogResult = DialogResult.OK,
            Font = new Font("Microsoft YaHei UI", 9F),
            BackColor = Color.FromArgb(55, 65, 81),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = { BorderSize = 0 },
            Location = new Point(264, 88),
            Size = new Size(100, 32),
            Cursor = Cursors.Hand
        };
        btnOk.Click += (_, _) =>
        {
            MainForm.SavePsConfigPath(string.IsNullOrWhiteSpace(_txtPsPath.Text) ? null : _txtPsPath.Text.Trim());
            Close();
        };

        var btnCancel = new Button
        {
            Text = "取消",
            DialogResult = DialogResult.Cancel,
            Font = new Font("Microsoft YaHei UI", 9F),
            Location = new Point(372, 88),
            Size = new Size(100, 32)
        };

        AcceptButton = btnOk;
        CancelButton = btnCancel;

        Controls.Add(lbl);
        Controls.Add(_txtPsPath);
        Controls.Add(_btnBrowse);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        var path = MainForm.LoadPsConfigPath();
        _txtPsPath.Text = string.IsNullOrWhiteSpace(path) ? "" : path;
    }

    private void BtnBrowse_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "选择 Photoshop 安装路径（Photoshop.exe）",
            Filter = "Photoshop.exe|Photoshop.exe|可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*",
            FileName = "Photoshop.exe",
            CheckFileExists = true
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        _txtPsPath.Text = dlg.FileName;
    }
}
