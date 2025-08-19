using System.Web;
using EasyNotification.Models;
//using MaterialDesignThemes.Wpf;
using ClassIsland.Shared.Helpers;
using ClassIsland.Core.Attributes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ClassIsland.Core.Models.Notification;
using ClassIsland.Core.Models.UriNavigation;
using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Core.Abstractions.Services.NotificationProviders;
using EasyNotification.Shared;
using System.IO;

namespace EasyNotification.Services.NotificationProviders;

[NotificationProviderInfo("AB67DC23-BEA9-40B7-912B-C7C37390A171", "EasyNotification",  "\ue02f", "由 EasyNotification 注册的提醒提供方，支持由 url 协议触发提醒。")]
public class EasyNotificationProvider : NotificationProviderBase, IHostedService
{

    private Settings Settings = new();
    public NotificationSettings NotificationSettings { get; set; } = new();

    private ILogger<EasyNotificationProvider> Logger;
    private INotificationHostService NotificationHostService { get; }

    public EasyNotificationProvider(INotificationHostService notificationHostService, IUriNavigationService uriNavigationService, ILogger<EasyNotificationProvider> logger)
    {
        NotificationHostService = notificationHostService;
        NotificationHostService.RegisterNotificationProvider(this);
        Logger = logger;
        uriNavigationService.HandlePluginsNavigation("easynotification/", Handler);
        uriNavigationService.HandlePluginsNavigation("en/", Handler);
        Settings = ConfigureFileHelper.LoadConfig<Settings>(Path.Combine(GlobalConstants.PluginConfigFolder, "Settings.json"));
        Settings.PropertyChanged += (sender, args) =>
        {
            ConfigureFileHelper.LoadConfig<Settings>(Path.Combine(GlobalConstants.PluginConfigFolder, "Settings.json"));

        };

    }

    private void Handler(UriNavigationEventArgs args)
    {
        string query = args.Uri.Query;
        var queryParams = HttpUtility.ParseQueryString(query);
        if (queryParams != null)
        {
            string dirValue = queryParams["dir"] ?? "";
            string type = queryParams["type"] ?? "simple";
            string secret = queryParams["secret"] ?? "";
            //Console.WriteLine(dirValue);
            Logger.LogDebug("传入的提醒配置文件路径为：\"{}\"", dirValue);
            if (System.IO.File.Exists(dirValue))
            {
                try
                {
                    NotificationSettings = ConfigureFileHelper.LoadConfigUnWrapped<NotificationSettings>(dirValue, false, false);
                }
                catch
                {
                    Logger.LogWarning("提醒设置文件加载失败，将忽略本次提醒请求。");
                    return;
                }
            }
            else
            {
                Logger.LogWarning("不存在的路径：\"{}\"，将忽略本次提醒请求。", dirValue);
                return;
            }

            if(Settings.Secret != "")
            {
                if(secret != Settings.Secret)
                {
                    Logger.LogWarning("Secret 错误，将忽略本次提醒请求。");
                    return;

                }
            }

            NotificationRequest NotificationRequest = new();
            switch (type) 
            {
                case "simple":
                    NotificationRequest = SimpleNotification(NotificationSettings); 
                    break;
                case "rolling":
                    NotificationRequest = RollingNotification(NotificationSettings);
                    break;
            }
            ShowNotification(NotificationRequest);
        }
        else
        {
            Logger.LogWarning("无效的 Uri 参数：\"{}\"，将忽略本次提醒请求。", args.Uri);
            return;
        }

    }
    
    private NotificationRequest SimpleNotification(NotificationSettings Settings)
    {
        var NotificationRequest = new NotificationRequest()
        {
            MaskContent = NotificationContent.CreateTwoIconsMask(Settings.MaskContent, " ", " ", false, x =>
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
        return NotificationRequest;
    }

    private NotificationRequest RollingNotification(NotificationSettings Settings)
    {
        var NotificationRequest = new NotificationRequest()
        {
            MaskContent = NotificationContent.CreateTwoIconsMask(Settings.MaskContent, " ", " ", false, x => { 
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
        return NotificationRequest;           
    }
}
