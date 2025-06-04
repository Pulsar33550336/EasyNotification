using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNotification.ViewModel;

public class SettingsPageViewModel : ObservableRecipient
{
    private bool _visible = false;
    private bool _invisible = true;
    public bool Visible
    {
        get => _visible;
        set
        {
            if (_visible != value)
            {
                _visible = value;
                OnPropertyChanged();
            }
        }
    }
    public bool InVisible
    {
        get => _invisible;
        set
        {
            if (_invisible != value)
            {
                _invisible = value;
                OnPropertyChanged();
            }
        }
    }
}