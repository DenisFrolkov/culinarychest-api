using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[Route("api/applicationUser")]
[ApiController]
public class ApplicationUserController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public ApplicationUserController(IRepositoryManager repository, ILoggerManager logger,
        IMapper mapper) //IRepositoryManager и ILoggerManager.
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet(template: "{userId}", Name = "GetApplicationUserByUserId")]
    public async Task<IActionResult> GetApplicationUser(int userId)
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
    public async Task<IActionResult> CreateApplicationUser([FromBody] CreateApplicationUserDtoDto createApplicationUser)
    {
        if (createApplicationUser == null)
        {
            _logger.LogError("ApplicationUserForCreationDto object sent from client is null.");
            return BadRequest("ApplicationUserForCreationDto object is null");
        }
        
        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid model state for the CreateApplicationUserDto object");
            return UnprocessableEntity(ModelState);
        }

        var applicationUserEntity = _mapper.Map<ApplicationUser>(createApplicationUser);
        _repository.ApplicationUser.CreateApplicationUser(applicationUserEntity);
        await _repository.SaveAsync();
        var applicationUserToReturn = _mapper.Map<ApplicationUserDto>(applicationUserEntity);
        return CreatedAtRoute("GetApplicationUserByUserId", new { Id = applicationUserToReturn.UserId },
            applicationUserToReturn);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteApplicationUser(int userId)
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
    public async Task<IActionResult> UpdateApplicationUser(int userId, [FromBody] UpdateApplicationUserDtoDto applicationUser)
    {
        if (applicationUser == null)
        {
            _logger.LogError("UpdateApplicationUserDto object sent from client is null.");
            return BadRequest("UpdateApplicationUserDto object is null"); 
        }

        var applicationUserEntity = await _repository.ApplicationUser.GetApplicationUser(userId, trackChanges: true);
        if (applicationUserEntity == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {userId} doesn't exist in the database.");
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid model state for the UpdateApplicationUserDto object");
            return UnprocessableEntity(ModelState);
        }

        _mapper.Map(applicationUser, applicationUserEntity);
        await _repository.SaveAsync();
        return NoContent();
    }
}

