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
    
    [HttpGet]
    public IActionResult GetApplicationUsers()
    {
        var applicationUsers = _repository.ApplicationUser.GetAllApplicationUsers(trackChanges: false);
        var applicationUserDto = _mapper.Map<IEnumerable<ApplicationUserDto>>(applicationUsers);
        return Ok(applicationUserDto);
    }
    
    [HttpGet(template: "{userId}", Name = "ApplicationUserByUserId")]
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
    public IActionResult CreateApplicationUsers([FromBody] ApplicationUserForCreationDto applicationUser)
    {
        if (applicationUser == null)
        {
            _logger.LogError("ApplicationUserForCreationDto object sent from client is null.");
            return BadRequest("ApplicationUserForCreationDto object is null");
        }
        var applicationUserEntity = _mapper.Map<ApplicationUser>(applicationUser);
        _repository.ApplicationUser.CreateApplicationUser(applicationUserEntity);
        _repository.Save();
        var applicationUserToReturn = _mapper.Map<ApplicationUserDto>(applicationUserEntity);
        return CreatedAtRoute("ApplicationUserByUserId", new { Id = applicationUserToReturn.UserId }, applicationUserToReturn);
    }
}

