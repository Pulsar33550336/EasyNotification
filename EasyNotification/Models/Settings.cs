using CommunityToolkit.Mvvm.ComponentModel;

namespace EasyNotification.Models;

public class Settings : ObservableRecipient
{
    private string _secret = "";

    public string Secret
    {
        get { return _secret; }
        set {
            if (value == _secret) return;
            _secret = value;
            OnPropertyChanged();
        }
    }
}
