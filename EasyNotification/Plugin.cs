using System.IO;
using EasyNotification.Models;
using ClassIsland.Shared.Helpers;
using ClassIsland.Core.Attributes;
using Microsoft.Extensions.Hosting;
using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Extensions.Registry;
using Microsoft.Extensions.DependencyInjection;
using EasyNotification.Services.NotificationProviders;
using EasyNotification.Views.SettingsPages;
using EasyNotification.Shared;

namespace EasyNotification;

[PluginEntrance]
public class Plugin : PluginBase
{
    public override void Initialize(HostBuilderContext context, IServiceCollection services)
    {
        //CommonDialog.ShowInfo("Hello world!");
        NotificationSettings NotificationSettings = new();
        services.AddNotificationProvider<EasyNotificationProvider>();
        services.AddSettingsPage<SettingsPage>();
        ConfigureFileHelper.SaveConfig<NotificationSettings>(Path.Combine(PluginConfigFolder, "Example.json"), NotificationSettings);
        GlobalConstants.PluginConfigFolder = PluginConfigFolder;
        
    }
}
