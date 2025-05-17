namespace EasyNotification.Models;

public class Settings
{
    private double _maskDuration = 5;
    private string _maskContent = "";
    private double _overlayDuration = 5;
    private string _overlayContent = "";
    private bool _isSpeechEnabled = false;
    private bool _isEffectEnabled = true;
    private bool _isSoundEnabled = false;
    private bool _isTopmost = true;

    public double MaskDuration
    {
        get => _maskDuration;
        set
        {
            _maskDuration = value;
        }
    }

    public string MaskContent
    {
        get => _maskContent;
        set
        {
            _maskContent = value;
        }
    }

    public double OverlayDuration
    {
        get => _overlayDuration;
        set
        {
            _overlayDuration = value;
        }
    }

    public string OverlayContent
    {
        get => _overlayContent;
        set
        {
            _overlayContent = value;
        }
    }

    public bool IsSpeechEnabled
    {
        get => _isSpeechEnabled;
        set
        {
            _isSpeechEnabled = value;

        }
    }
    public bool IsEffectEnabled
    {
        get => _isEffectEnabled;
        set
        {
            _isEffectEnabled = value;

        }
    }
    public bool IsSoundEnabled
    {
        get => _isSoundEnabled;
        set
        {
            _isSoundEnabled = value;

        }
    }
    public bool IsTopmost
    {
        get => _isTopmost;
        set
        {
            _isTopmost = value;

        }
    }
}
