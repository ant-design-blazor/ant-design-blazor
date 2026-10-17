namespace AntDesign.Docs.Demos.Components.Form.demo;

public sealed class RegisterFormModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Confirm { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Residence { get; set; } = string.Empty;
    public string PhonePrefix { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string Intro { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Captcha { get; set; } = string.Empty;
    public bool Agreement { get; set; }
}
