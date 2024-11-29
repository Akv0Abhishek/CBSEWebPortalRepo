using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CBSEWebPortal.Models;
using CBSEWebPortal.Services;


[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ICBSEAdminService _cbseAdminService;
    private readonly IPrincipalService _principalService;
    private readonly IConfiguration _configuration;

    public LoginController(ICBSEAdminService cbseAdminService, IPrincipalService principalService, IConfiguration configuration)
    {
        _cbseAdminService = cbseAdminService;
        _principalService = principalService;
        _configuration = configuration;
    }

    [HttpPost("admin")]
    public async Task<IActionResult> LoginAdmin([FromBody] LoginRequest loginRequest)
    {
        if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            return BadRequest("Invalid login request.");

        var admin = await _cbseAdminService.GetAdminByEmailAsync(loginRequest.Email);
        if (admin == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, admin.Password))
            return Unauthorized("Invalid credentials.");

        var token = GenerateJwtToken(admin.Email, "admin");
        return Ok(new { Token = token, Role = "admin" });
    }

    [HttpPost("principal")]
    public async Task<IActionResult> LoginPrincipal([FromBody] LoginRequest loginRequest)
    {
        if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            return BadRequest("Invalid login request.");

        var principal = await _principalService.GetPrincipalByEmailAsync(loginRequest.Email);
        if (principal == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, principal.Password))
            return Unauthorized("Invalid credentials.");

        var token = GenerateJwtToken(principal.Email, "Principal");
        return Ok(new { Token = token, Role = "Principal", PrincipalId = principal.ID });
    }

    private string GenerateJwtToken(string email, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
