using AutoMapper;
using Contracts;
using culinarychest_api.ActionFilters;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[ApiVersion("1.0")]
[Route("api/applicationUser")]
[ApiController]
public class ApplicationUserController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public ApplicationUserController(IRepositoryManager repository, ILoggerManager logger,
        IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet(template: "{userId}", Name = "GetApplicationUserByUserId")]
    public async Task<IActionResult> GetApplicationUser(string userId)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(userId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {userId} doesn't exist in the database.");
            return NotFound();
        }
        else
        {
            var applicationUserDto = _mapper.Map<ApplicationUserDto>(applicationUser);
            return Ok(applicationUserDto);
        }
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateApplicationUser([FromBody] CreateApplicationUserDtoDto createApplicationUser)
    {
        var applicationUserEntity = _mapper.Map<ApplicationUser>(createApplicationUser);
        _repository.ApplicationUser.CreateApplicationUser(applicationUserEntity);
        await _repository.SaveAsync();
        var applicationUserToReturn = _mapper.Map<ApplicationUserDto>(applicationUserEntity);
        return CreatedAtRoute("GetApplicationUserByUserId", new { Id = applicationUserToReturn.UserId },
            applicationUserToReturn);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteApplicationUser(string userId)
    {
        var applicationUser = await _repository.ApplicationUser.GetApplicationUser(userId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {userId} doesn't exist in the database.");
            return NotFound();
        }   
        _repository.ApplicationUser.DeleteApplicationUser(applicationUser);
        await _repository.SaveAsync();
        return NoContent();
    }

    [HttpPut("{userId}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateApplicationUser(string userId, [FromBody] UpdateApplicationUserDtoDto applicationUser)
    {
        var applicationUserEntity = await _repository.ApplicationUser.GetApplicationUser(userId, trackChanges: true);
        if (applicationUserEntity == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {userId} doesn't exist in the database.");
            return NotFound();
        }
        _mapper.Map(applicationUser, applicationUserEntity);
        await _repository.SaveAsync();
        return NoContent();
    }
}

