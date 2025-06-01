# EasyNotification

一个简单的插件，可以通过 uri 协议与 json 文件调用 ClassIsland 的提醒功能。

您可以调用以下 url 来触发提醒（前提是 ClassIsland 注册了 uri 协议）：

```
classisland://plugins/easynotification/?dir=[在这里填入你的 json 文件的绝对路径]&type=[simple|rolling]
```

- `simple` : 普通提醒
- `rolling` : 滚动提醒

您可以在本插件的设置目录下找到示例文件。**注意：本文件每次启动时会被覆盖。**

示例：

```
{
	"MaskDuration":5,
	"MaskContent":"123",
	"OverlayDuration":5,
	"OverlayContent":"456",
	"IsSpeechEnabled":false,
	"IsEffectEnabled":true,
	"IsSoundEnabled":true,
	"IsTopmost":true
}
```

- `MaskDuration`：提醒遮罩持续时间
- `MaskContent`：提醒遮罩内容
- `OverlayDuration`：提醒正文持续时间
- `OverlayContent`：提醒正文内容
- `IsSpeechEnabled`：启用语音
- `IsEffectEnabled`：启用水波纹特效
- `IsSoundEnabled`：启用提醒声音
- `IsTopmost`：启用强制置顶
