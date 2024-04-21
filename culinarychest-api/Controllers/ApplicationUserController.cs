using System.Security.Claims;
using AutoMapper;
using Contracts;
using culinarychest_api.ActionFilters;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[Route("api/authentication")]
[ApiController]
public class ApplicationUserController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthenticationManager _authManager;

    public ApplicationUserController(ILoggerManager logger, IMapper mapper, UserManager<ApplicationUser> userManager,
        IAuthenticationManager authManager)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _authManager = authManager;
    }
    
    [HttpPost("register")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] RegistrationApplicationUserDto registrationApplicationUser)
    {
        var user = _mapper.Map<ApplicationUser>(registrationApplicationUser);
        var result = await _userManager.CreateAsync(user, registrationApplicationUser.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        await _userManager.AddToRolesAsync(user, registrationApplicationUser.Roles);
        return StatusCode(201);
    }
    
    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticationApplicationUserDto authenticationApplicationUser)
    {
        if (!await _authManager.ValidateUser(authenticationApplicationUser))
        {
            _logger.LogWarn($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
            return Unauthorized();
        }
        return Ok(new { Token = await _authManager.CreateToken() });
    }
    
    [HttpGet("user"), Authorize]
    public async Task<IActionResult> GetUserId()
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(new { UserId = user.Id });
    }
}

