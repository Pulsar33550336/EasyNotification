using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Enums.SettingsWindow;
using ClassIsland.Shared.Helpers;
using EasyNotification.Models;
using EasyNotification.Shared;
using Avalonia.Interactivity;
using System;
using System.ComponentModel;
using System.IO;
using ClassIsland.Core.Abstractions.Services;
using EasyNotification.ViewModel;

namespace EasyNotification.Views.SettingsPages;

[SettingsPageInfo("easynotification.settingpage", "EasyNotification", "\ue02f", "\ue02e",
    SettingsPageCategory.External)]
public partial class SettingsPage : SettingsPageBase
{
    public Settings Settings { get; set; } = new();

    public SettingsPageViewModel ViewModel { get; } = new();
    
    private IUriNavigationService UriNS;
    public SettingsPage(IUriNavigationService uriNavigationService)
    {

        Settings = ConfigureFileHelper.LoadConfig<Settings>(Path.Combine(GlobalConstants.PluginConfigFolder,
            "Settings.json"));
        Settings.PropertyChanged += (sender, args) =>
        {
            ConfigureFileHelper.SaveConfig<Settings>(
                Path.Combine(GlobalConstants.PluginConfigFolder, "Settings.json"), Settings);
        };
        InitializeComponent();
        UriNS = uriNavigationService;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Random random = new();
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] stringChars = new char[20];
        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        Settings.Secret = new(stringChars);
    }

    private void Visible_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Visible = true;
        ViewModel.InVisible = false;
    }
}