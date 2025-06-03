using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Attributes;
using ClassIsland.Shared.Helpers;
using EasyNotification.Models;
using EasyNotification.Shared;
using System.IO;

namespace EasyNotification.Views.SettingsPages;

[SettingsPageInfo("easynotification.settingpage", "示例设置页面")]
public partial class SettingsPage : SettingsPageBase
{
    public Settings Settings = new();

    public SettingsPage()
    {
        
        Settings = ConfigureFileHelper.LoadConfig<Settings>(Path.Combine(GlobalConstants.PluginConfigFolder, "Settings.json"));
        Settings.PropertyChanged += (sender, args) =>
        {
            ConfigureFileHelper.SaveConfig<Settings>(Path.Combine(GlobalConstants.PluginConfigFolder, "Settings.json"), Settings);
        };
        InitializeComponent();
    }
}