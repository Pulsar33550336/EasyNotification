
using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Controls.CommonDialog;
using ClassIsland.Core.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EasyNotification.Services.NotificationProviders;
using ClassIsland.Core.Extensions.Registry;
using EasyNotification.Models;
using ClassIsland.Shared.Helpers;
using System.IO;

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
