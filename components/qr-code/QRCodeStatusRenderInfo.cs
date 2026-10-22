using System;
using System.Threading.Tasks;

namespace AntDesign;

/// <summary>Information available to a custom QR code status template.</summary>
public sealed class QRCodeStatusRenderInfo
{
    internal QRCodeStatusRenderInfo(string status, QRCodeLocale locale, Func<Task> onRefresh)
    {
        Status = status;
        Locale = locale;
        OnRefresh = onRefresh;
    }

    public string Status { get; }
    public QRCodeLocale Locale { get; }
    public Func<Task> OnRefresh { get; }
}
