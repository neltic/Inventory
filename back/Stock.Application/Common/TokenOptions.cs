using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Application.Common;

public class TokenOptions
{
    public const string SectionName = "Token";
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; }
}
