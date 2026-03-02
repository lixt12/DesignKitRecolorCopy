# 设计工具集（美图换色 / Photopea）

桌面 EXE 内嵌多个在线设计工具，一个窗口即可切换使用：

- **美图换色**：打开 [美图设计室 - AI 服装换色](https://www.designkit.cn/recolor/?from=tools)，上传图片并选好换色区域后，可一键从页面提取换色结果图并**以图片形式**复制到剪贴板。
- **Photopea(PS)**：打开 [Photopea](https://www.photopea.com/) 在线修图（类 Photoshop），直接使用网页内功能即可。

## 环境要求

- **Windows 10/11**（x64 或 x86）
- **.NET 8 SDK**（开发/构建）
- **WebView2 运行时**（运行 EXE 时需要，多数 Windows 11 已自带；若未安装可从 [Microsoft 官网](https://developer.microsoft.com/microsoft-edge/webview2/) 下载：**常青引导程序** 点「下载↓」即可，联网自动安装；需离线安装时选 **常青独立安装程序** 里的 **x64 ↓**）

## 构建与运行

### 开发运行

```bash
cd D:\IdeaProjects\12\DesignKitRecolorCopy
dotnet run
```

### 发布为单机 EXE

在项目目录下对**项目**发布（不要对 .sln 使用 `-o`，否则会触发 NETSDK1194 警告）。

**方式一：多文件（体积小，需用户已装 .NET 8）**

```bash
cd D:\IdeaProjects\12\DesignKitRecolorCopy
dotnet publish DesignKitRecolorCopy.csproj -c Release -r win-x64 --self-contained false -o ./publish
```

**方式二：单文件 EXE（推荐分发给用户）—— 全部打进一个 exe**

```bash
cd D:\IdeaProjects\12\DesignKitRecolorCopy
dotnet publish DesignKitRecolorCopy.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ./publish
```

（`IncludeNativeLibrariesForSelfExtract=true` 会把 WebView2 加载器等原生 DLL 打进 exe 并在首次运行时释放，避免用户报「Dll was not found」。）

- `win-x64` 可改为 `win-x86`。
- 单文件发布会把 .NET 运行时和本程序所有依赖都打进 **一个** `DesignKitRecolorCopy.exe`（约 60～80MB），用户**无需安装 .NET**，解压/复制到任意位置双击即可运行。
- 用户电脑仍需 **WebView2 运行时**（Win11 多数已带；[下载](https://developer.microsoft.com/microsoft-edge/webview2/)）。缺 WebView2 时程序会提示或无法打开内嵌网页。

发布后运行：`publish\DesignKitRecolorCopy.exe`。

### 分发给用户

- **若用上面的「单文件 EXE」发布**：只需把 `publish` 里的 **一个** `DesignKitRecolorCopy.exe` 发给用户（或打成 zip），用户放到任意目录双击即可，无需再带一堆 DLL、也无需安装 .NET。
- **若用「多文件」发布**：必须把整个 **`publish` 文件夹** 打包发给用户，让用户在该文件夹内双击 exe；且用户需已安装 [.NET 8 桌面运行时](https://dotnet.microsoft.com/download/dotnet/8.0)。
- **无论哪种方式**：用户电脑都需要 **WebView2 运行时**（多数 Windows 11 已自带）。
- **若用户双击无反应**：让用户在该目录打开 CMD 执行 `DesignKitRecolorCopy.exe` 看报错，或检查是否缺 WebView2。
- **若提示「Dll was not found」且用户已安装 WebView2**：改用 **方式一（多文件）** 发布，把整个 `publish` 文件夹打包给用户，让用户在该文件夹内运行 exe；或让用户安装 WebView2 **常青独立安装程序** 的 **x64** 版本（不要只装引导程序）。

## 使用步骤

1. 启动程序，自动打开美图换色页。
2. 在页面内：上传图片 → 选择换色区域（如上衣）→ 等待生成换色结果。
3. 点击顶部 **「一键复制换色图」**。
4. 若成功，状态栏会显示“已复制换色图到剪贴板”，剪贴板中为**图片**，可直接粘贴到 Word、微信、画图等任意支持粘贴图片的应用中。

若未找到换色图，会提示“未找到换色结果图，请先上传图片并选择换色区域”。

## 技术说明

- **技术栈**：C# + WinForms + WebView2（Microsoft.Web.WebView2）。
- **取图逻辑**：在页面中执行注入脚本，查找 `img[src^="data:image/png;base64"]` 及带 `ai-cp-image` 的容器内图片，取面积最大的一张作为换色结果图，通过 `window.chrome.webview.postMessage` 将 `src`（data URL）回传宿主；宿主解析 Base64 为图片后，用 `Clipboard.SetImage` 以**图片**形式写入剪贴板。
- 若美图页面改版导致选择器失效，可修改 `MainForm.cs` 中的 `GetRecolorImageScript` 以适配新 DOM 结构。

## 项目结构

```
DesignKitRecolorCopy/
├── DesignKitRecolorCopy.csproj
├── DesignKitRecolorCopy.sln
├── Program.cs
├── MainForm.cs
├── MainForm.Designer.cs
└── README.md
```
