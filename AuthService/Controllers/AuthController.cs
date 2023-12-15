using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _config;
    IVaultClient vaultClient;

    public AuthController(ILogger<AuthController> logger, IConfiguration config)
    {
        _config = config;
        _logger = logger;
        // Vault
        var EndPoint = Environment.GetEnvironmentVariable("vault") ?? "https://localhost:8201/";
        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback = (
            message,
            cert,
            chain,
            sslPolicyErrors
        ) =>
        {
            return true;
        };

        // Initialize one of the several auth methods.
        IAuthMethodInfo authMethod = new TokenAuthMethodInfo(
            "00000000-0000-0000-0000-000000000000"
        );
        // Initialize settings. You can also set proxies, custom delegates etc. here.
        var vaultClientSettings = new VaultClientSettings(EndPoint, authMethod)
        {
            Namespace = "",
            MyHttpClientProviderFunc = handler =>
                new HttpClient(httpClientHandler) { BaseAddress = new Uri(EndPoint) }
        };
        vaultClient = new VaultClient(vaultClientSettings);
    }

    private async Task<string> GenerateJwtToken(string username)
    {
        

        // Use client to read a key-value secret.
        Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
            path: "auctionSecrets",
            mountPoint: "secret"
        );
        string mySecret = kv2Secret.Data.Data["Secret"].ToString();
        _logger.LogInformation($"### mySecret {mySecret}");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mySecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, username) };

        var token = new JwtSecurityToken(
            _config["Issuer"],
            "http://localhost",
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        _logger.LogInformation(login.Username);
        // Erstat med din egen godkendelses-logik!
        if (login.Username != "username" || login.Password != "password")
        {
            return Unauthorized();
        }
        var token = GenerateJwtToken(login.Username);
        return Ok(new {token});
    }

    [AllowAnonymous]
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateJwtToken([FromBody] TokenDTO token)
    {
        if (token.Token.IsNullOrEmpty())
            return BadRequest("Invalid token submitted.");
        var tokenHandler = new JwtSecurityTokenHandler();
        Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
            path: "auctionSecrets",
            mountPoint: "secret"
        );
        string mySecret = kv2Secret.Data.Data["Secret"].ToString();

        var key = Encoding.ASCII.GetBytes(mySecret);
        try
        {
            tokenHandler.ValidateToken(
                token.Token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },
                out SecurityToken validatedToken
            );
            var jwtToken = (JwtSecurityToken)validatedToken;
            var accountId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return Ok(accountId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(404);
        }
    }
    public class TokenDTO
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
