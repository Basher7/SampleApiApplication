using Domain.Entities.Jwe;
using Domain.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstraction;

public sealed class JweTokenUtils
{
    /// <summary>
    /// Generate Json Web Token using JWE mechanism. In Jwe mechanism encryption key must be 128bits(string Length 16), 256bits(string Length 32)
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static string GenerateJWEToken()
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthKeys.tokenSignKey));
        string algorithm = SecurityAlgorithms.HmacSha256;
        var signingCredentials = new SigningCredentials(signingKey, algorithm);

        var encryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthKeys.tokenScreatKey));
        var encryptCredentials = new EncryptingCredentials(encryptionKey, SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512);

        DateTime nowDate = DateTime.UtcNow;
        DateTime expireDate = nowDate.AddMinutes(5);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = expireDate,
            Issuer = AuthKeys.issuer,
            Audience = AuthKeys.audience,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptCredentials
        };

        var tokenHandler = new JsonWebTokenHandler();
        string token = tokenHandler.CreateToken(tokenDescriptor);

        return token;
    }


    public static async Task<GlobalApiResponse> ValidateJweToken(string _sessionToken, HttpContext context)
    {
        string sToken = _sessionToken;

        TokenValidationResult tokenValidationResult = await GetJwePrincipal(sToken);
        string failedMsg = MessageCollections.InvalidSession;

        if (!tokenValidationResult.IsValid && tokenValidationResult.Exception is not null)
        {
            string errMsg = tokenValidationResult.Exception.ExceptionMessage();
            if (errMsg.StartsWith("IDX10223: Lifetime validation failed"))
            {
                string sessionExpiredMsg = MessageCollections.InvalidSession;
                string? errorDetails = tokenValidationResult.Exception.StackTrace;
                return new GlobalApiResponse(true, sessionExpiredMsg, null, errorDetails);
            }

            string? errDetails = tokenValidationResult.Exception.StackTrace;
            return new GlobalApiResponse(true, failedMsg, null, errDetails);
        }

        if (tokenValidationResult?.ClaimsIdentity is not ClaimsIdentity identity)
            return new GlobalApiResponse(true, failedMsg, null, "Claims Identity is Null.");

        if (!identity.IsAuthenticated)
            return new GlobalApiResponse(true, failedMsg, null, "Claims Identity authorization failed.");

        string? loginProviderId = identity.FindFirst("jti")?.Value;

        context.User = new ClaimsPrincipal(identity);
        return new GlobalApiResponse(false, MessageCollections.Success, loginProviderId, "Validation Success.");
    }


    public static TokenValidationParameters GetJweTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidIssuer = AuthKeys.issuer,
            ValidateAudience = true,
            ValidAudience = AuthKeys.audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthKeys.tokenSignKey)),
            TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthKeys.tokenScreatKey))
        };
    }


    private static async Task<TokenValidationResult> GetJwePrincipal(string sToken)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var validationParameters = GetJweTokenValidationParameters();

        TokenValidationResult result = await tokenHandler.ValidateTokenAsync(sToken, validationParameters);

        return result;
    }

}
