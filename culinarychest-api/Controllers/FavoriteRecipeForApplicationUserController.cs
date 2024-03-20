using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace culinarychest_api.Controllers;

[Route("api/applicationUser/{authorId}/favoriteRecipe")]
[ApiController]
public class FavoriteRecipeForApplicationUserController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger; 
    private readonly IMapper _mapper;

    
    public FavoriteRecipeForApplicationUserController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository; 
        _logger = logger; 
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetFavoriteRecipeForApplicationUser(int authorId)
    {
        var applicationUser = _repository.ApplicationUser.GetApplicationUser(authorId, trackChanges: false);
        if (applicationUser == null)
        {
            _logger.LogInfo($"ApplicationUser with id: {authorId} doesn't exist in the database.");
            return NotFound();
        }
        var favoriteRecipeFromDb = _repository.FavoriteRecipe.GetAllFavoriteRecipeForApplicationUser(authorId, trackChanges: false);
        var favoriteRecipeDto = _mapper.Map<IEnumerable<FavoriteRecipeDto>>(favoriteRecipeFromDb);
        return Ok(favoriteRecipeDto);
    }
}