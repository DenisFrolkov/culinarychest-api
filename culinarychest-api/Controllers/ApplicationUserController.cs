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
    
    public ApplicationUserController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) //IRepositoryManager и ILoggerManager.
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
    
    [HttpGet(template: "{userId}", Name = "GetApplicationUserByUserId")]
    public IActionResult GetApplicationUser(int userId)
    {
        var applicationUser = _repository.ApplicationUser.GetApplicationUser(userId, trackChanges: false);
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
    public IActionResult CreateApplicationUser([FromBody] CreateApplicationUserDto createApplicationUser)
    {
        if (createApplicationUser == null)
        {
            _logger.LogError("ApplicationUserForCreationDto object sent from client is null.");
            return BadRequest("ApplicationUserForCreationDto object is null");
        }
        var applicationUserEntity = _mapper.Map<ApplicationUser>(createApplicationUser);
        _repository.ApplicationUser.CreateApplicationUser(applicationUserEntity);
        _repository.Save();
        var applicationUserToReturn = _mapper.Map<ApplicationUserDto>(applicationUserEntity);
        return CreatedAtRoute("GetApplicationUserByUserId", new { Id = applicationUserToReturn.UserId }, applicationUserToReturn);
    }
}

