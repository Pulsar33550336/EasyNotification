using System.Web;
using EasyNotification.Models;
using MaterialDesignThemes.Wpf;
using ClassIsland.Shared.Helpers;
using ClassIsland.Core.Attributes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ClassIsland.Core.Models.Notification;
using ClassIsland.Core.Models.UriNavigation;
using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Core.Abstractions.Services.NotificationProviders;

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
        uriNavigationService.HandlePluginsNavigation("easynotification/",Handler);
    }

    private void Handler(UriNavigationEventArgs args)
    {
        string query = args.Uri.Query;
        var queryParams = HttpUtility.ParseQueryString(query);
        if (queryParams != null)
        {
            string dirValue = queryParams["dir"] ?? "";
            string type = queryParams["type"] ?? "simple";
            //Console.WriteLine(dirValue);
            Logger.LogDebug("传入的提醒配置文件路径为：\"{}\"", dirValue);
            switch (type) 
            {
                case "simple":
                    SimpleNotification(dirValue); 
                    break;
                case "rolling":
                    RollingNotification(dirValue);
                    break;
            }
            
        }
        else
        {
            Logger.LogWarning("无效的 Uri 参数：\"{}\"，将忽略本次提醒请求。", args.Uri);
        }

    }
    
    private void SimpleNotification(string dirValue)
    {
        if(System.IO.File.Exists(dirValue))
        {
            try
            {
                Settings = ConfigureFileHelper.LoadConfigUnWrapped<Settings>(dirValue, false, false);
                var NotificationRequest = new NotificationRequest()
                {
                    MaskContent = NotificationContent.CreateTwoIconsMask(Settings.MaskContent, PackIconKind.BellOutline, 0, false, x =>
                    {
                        x.Duration = TimeSpan.FromSeconds(Settings.MaskDuration);
                    }),
                    OverlayContent = NotificationContent.CreateSimpleTextContent(Settings.OverlayContent, x =>
                    {
                        x.Duration = TimeSpan.FromSeconds(Settings.OverlayDuration);
                    }),
                    RequestNotificationSettings =
                    {
                        IsSettingsEnabled = true,
                        IsSpeechEnabled = Settings.IsSpeechEnabled,
                        IsNotificationEffectEnabled = Settings.IsEffectEnabled,
                        IsNotificationSoundEnabled = Settings.IsSoundEnabled,
                        IsNotificationTopmostEnabled = Settings.IsTopmost
                    }
                };
                ShowNotification(NotificationRequest);
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

    private void RollingNotification(string dirValue)
    {
        if (System.IO.File.Exists(dirValue))
        {
            try
            {
                Settings = ConfigureFileHelper.LoadConfigUnWrapped<Settings>(dirValue, false, false);
                var NotificationRequest = new NotificationRequest()
                {
                    MaskContent = NotificationContent.CreateTwoIconsMask(Settings.MaskContent, PackIconKind.BellOutline, 0, false, x => { 
                        x.Duration = TimeSpan.FromSeconds(Settings.MaskDuration); 
                    }),
                    OverlayContent = NotificationContent.CreateRollingTextContent(Settings.OverlayContent,TimeSpan.FromSeconds(Settings.OverlayDuration)),
                    RequestNotificationSettings =
                    {
                        IsSettingsEnabled = true,
                        IsSpeechEnabled = Settings.IsSpeechEnabled,
                        IsNotificationEffectEnabled = Settings.IsEffectEnabled,
                        IsNotificationSoundEnabled = Settings.IsSoundEnabled,
                        IsNotificationTopmostEnabled = Settings.IsTopmost
                    }
                };
                ShowNotification(NotificationRequest);
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
}
