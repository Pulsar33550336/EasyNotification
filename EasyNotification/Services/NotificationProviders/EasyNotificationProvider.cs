using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Shared.Interfaces;
using Microsoft.Extensions.Hosting;
using ClassIsland.Core.Controls.CommonDialog;
using ClassIsland.Shared.Helpers;
using System.Web;
using System;
using Microsoft.Extensions.Logging;
using EasyNotification.Models;
using ClassIsland.Core.Models.Notification;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using MaterialDesignThemes.Wpf;
using ClassIsland.Core.Abstractions.Services.NotificationProviders;
using ClassIsland.Core.Attributes;
using System.IO;


namespace EasyNotification.Services.NotificationProviders;

[NotificationProviderInfo("AB67DC23-BEA9-40B7-912B-C7C37390A171", "EasyNotification", PackIconKind.Notifications, "由 EasyNotification 注册的提醒提供方，支持由 url 协议触发提醒。")]
public class EasyNotificationProvider : NotificationProviderBase, IHostedService
{

    public Settings Settings { get; set; } = new();

    private ILogger<EasyNotificationProvider> Logger;
    private INotificationHostService NotificationHostService { get; }

    public EasyNotificationProvider(INotificationHostService notificationHostService, IUriNavigationService uriNavigationService, ILogger<EasyNotificationProvider> logger)
    {
        NotificationHostService = notificationHostService;
        NotificationHostService.RegisterNotificationProvider(this);
        Logger = logger;
        uriNavigationService.HandlePluginsNavigation(
        "easynotification/",
        args =>
        {
            string query = args.Uri.Query;
            var queryParams = HttpUtility.ParseQueryString(query);
            if (queryParams != null)
            {
                string dirValue = queryParams["dir"] ?? "";
                //Console.WriteLine(dirValue);
                Logger.LogDebug("传入的提醒配置文件路径为：\"{}\"", dirValue);
                if (System.IO.File.Exists(dirValue))
                {
                    try
                    {
                        Settings = ConfigureFileHelper.LoadConfigUnWrapped<Settings>(dirValue, false, false);
                        ShowNotification(BuildNotification());
                    }
                    catch
                    {
                        Logger.LogWarning("提醒设置文件加载失败，将忽略本次提醒请求。");
                    }
                }
                else
                {
                    Logger.LogWarning("不存在的路径：\"{}\"，将忽略本次提醒请求。", dirValue);
                }
            }
            else
            {
                Logger.LogWarning("无效的 Uri 参数：\"{}\"，将忽略本次提醒请求。", args.Uri);
            }

        });
    }

    private NotificationRequest BuildNotification()
    {
        var onNotificationRequest = new NotificationRequest()
        {
            MaskContent = NotificationContent.CreateSimpleTextContent(Settings.MaskContent),
            OverlayContent = NotificationContent.CreateSimpleTextContent(Settings.OverlayContent),
            RequestNotificationSettings =
            {
                IsSettingsEnabled = true,
                IsSpeechEnabled = Settings.IsSpeechEnabled,
                IsNotificationEffectEnabled = Settings.IsEffectEnabled,
                IsNotificationSoundEnabled = Settings.IsSoundEnabled,
                IsNotificationTopmostEnabled = Settings.IsTopmost
            }

        };
        return onNotificationRequest;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}
