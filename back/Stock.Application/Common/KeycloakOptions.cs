namespace Stock.Application.Common;

public class KeycloakOptions
{
    public const string SectionName = "Keycloak";
    public string Authority { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string MetadataAddress { get; set; } = string.Empty;
    public int ClockSkewSeconds { get; set; } = 0;
}