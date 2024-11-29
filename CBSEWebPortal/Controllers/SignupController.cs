using Microsoft.AspNetCore.Mvc;
using CBSEWebPortal.Models;
using CBSEWebPortal.Services;

[Route("api/[controller]")]
[ApiController]
public class SignupController : ControllerBase
{
    private readonly ICBSEAdminService _cbseAdminService;

    private readonly IPrincipalService _principalService;

    public SignupController(ICBSEAdminService cbseAdminService, IPrincipalService principalService)
    {
        _cbseAdminService = cbseAdminService;
        _principalService = principalService;
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> AddAdmin([FromBody] CBSEAdmin admin)
    {
        if (admin == null)
            return BadRequest(new { Message = "Admin details are null." });

        var result = await _cbseAdminService.AddAdminAsync(admin);

        if (!result)
            return StatusCode(500, new { Message = "An error occurred while saving admin details." });

        return Ok(new { Message = "Admin added successfully." });
    }

    [HttpPost("register-principal")]
    public async Task<IActionResult> AddPrincipal([FromBody] RegisterPrincipalRequest principal)
    {
        if (principal == null)
        {
            return BadRequest(new { message = "Principal data is null." });
        }

        var result = await _principalService.AddPrincipalAsync(principal);
        if (!result)
            return StatusCode(500, new { Message = "An error occurred while saving principal details." });

        return Ok(new { message = "Principal registered successfully" });
    }

}

