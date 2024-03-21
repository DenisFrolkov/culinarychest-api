using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[Route("api/applicationUser/{authorId}/recipe")]
[ApiController]
public class RecipeForApplicationUserController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public RecipeForApplicationUserController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet(Name = "RecipeForApplicationUserByAuthorId")]
    public IActionResult GetRecipeForApplicationUser(int authorId)
    {
        var applicationUser = _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        var recipesDb = _repository.Recipe.GetApplicationUserAllRecipes(authorId, trackChanges: false);
        var recipesDto = _mapper.Map<IEnumerable<RecipeDto>>(recipesDb);
        return Ok(recipesDto);
    }
}