namespace goida.Services;

public class JwtOptions
{
    public string Issuer { get; set; } = "goida";
    public string Audience { get; set; } = "goida";
    public string Key { get; set; } = "dev_super_secret_key_change_me";
    public int ExpiresMinutes { get; set; } = 120;
}
