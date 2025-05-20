using System.IO;
using EasyNotification.Models;
using ClassIsland.Shared.Helpers;
using ClassIsland.Core.Attributes;
using Microsoft.Extensions.Hosting;
using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Extensions.Registry;
using Microsoft.Extensions.DependencyInjection;
using EasyNotification.Services.NotificationProviders;

namespace EasyNotification;

[PluginEntrance]
public class Plugin : PluginBase
{
    public override void Initialize(HostBuilderContext context, IServiceCollection services)
    {
        //CommonDialog.ShowInfo("Hello world!");
        Settings Settings = new();
        services.AddNotificationProvider<EasyNotificationProvider>();
        ConfigureFileHelper.SaveConfig<Settings>(Path.Combine(PluginConfigFolder, "Example.json"), Settings);
    }
}
