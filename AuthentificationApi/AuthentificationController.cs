using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TP5.Shared;

namespace AuthentificationApi;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    // pour lire la cle  dans appsettings.json
    private readonly IConfiguration _configuration;

    // pour appeler l'API userbylogin de l'api
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthenticationController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    // renvoie un token JWT pour cet utilisateur
    [HttpPost]
    public async Task<IActionResult> GenererToken(string login)
    {
        // get les infos de l'user dans l'api
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://gestionordinateurs-api:5108/api/Users/login/{login}");

        // 'utilisateur n'existe pas
        if (!response.IsSuccessStatusCode)
        {
            return NotFound($"L'utilisateur {login} n'existe pas");
        }

        var user = await response.Content.ReadFromJsonAsync<User>();
        if (user == null)
        {
            return NotFound($"L'utilisateur {login} n'existe pas");
        }

        // claims login et role de l'user
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        // secret key
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? ""));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // token valable 15 min
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials);

        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
}