
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Jwe;

public sealed class AuthKeys
{
    [Required] public static string tokenSignKey { get; set; } = string.Empty;
    [Required] public static string tokenScreatKey { get; set; } = string.Empty;
    [Required] public static string audience { get; set; } = string.Empty;
    [Required] public static string issuer { get; set; } = string.Empty;
}
