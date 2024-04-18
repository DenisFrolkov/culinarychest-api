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
public class AuthenticationController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IAuthenticationManager _authManager;

    public AuthenticationController(ILoggerManager logger, IMapper mapper, UserManager<User> userManager,
        IAuthenticationManager authManager)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _authManager = authManager;
    }
    
    [HttpPost("register")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
    {
        var user = _mapper.Map<User>(userForRegistration);
        var result = await _userManager.CreateAsync(user, userForRegistration.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
        return StatusCode(201);
    }
    
    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
    {
        if (!await _authManager.ValidateUser(user))
        {
            _logger.LogWarn($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
            return Unauthorized();
        }
        return Ok(new { Token = await _authManager.CreateToken() });
    }
    
    [HttpGet("user"), Authorize]
    public IActionResult GetUserName()
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);

        return Ok(new { User = userName});
    }
    
    [HttpGet("userInfo/{userName}"), Authorize]
    public async Task<IActionResult> GetUserInfo(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(new { UserId = user.Id, Email = user.Email });
    }
}

