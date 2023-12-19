using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Security.KeyVault.Keys;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    IConfiguration configuration;
    SecretClient secretClient;
    public AuthController(IConfiguration configuration)
    {
        this.configuration = configuration;

        string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
        var kvUri = "https://" + keyVaultName + ".vault.azure.net";

        secretClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Auth([FromBody] User user)
    {
        IActionResult response = Unauthorized();

        if (user != null)
        {
            if (user.UserName.Equals("testuser") && user.Password.Equals("testpw"))
            {
                var secret = secretClient.GetSecret("Secret");
                var issuer = secretClient.GetSecret("Issuer");
                var audience = secretClient.GetSecret("Audience");
                var key = Encoding.UTF8.GetBytes(secret.Value.Value);
                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature
                );

                var subject = new ClaimsIdentity(new[]
                {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                    });

                var expires = DateTime.UtcNow.AddMinutes(10);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = subject,
                    Expires = expires,
                    Issuer = issuer.Value.Value,
                    Audience = audience.Value.Value,
                    SigningCredentials = signingCredentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return Ok(jwtToken);
            }
        }

        return response;
    }

    [Authorize]
    [HttpGet]
    public Task<ActionResult> GetTest()
    {
        return Task.FromResult<ActionResult>(Ok("AuthTest is running..."));
    }
}
