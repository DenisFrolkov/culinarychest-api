using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace culinarychest_api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FavoriteRecipe, FavoriteRecipeDto>();
        CreateMap<Recipe, RecipeDto>();
        CreateMap<Step, StepDto>();

        CreateMap<CreateFavoriteRecipeDtoDto, FavoriteRecipe>();
        CreateMap<CreateRecipeDto, Recipe>();
        CreateMap<CreateStepsDto, Step>();

        CreateMap<UpdateRecipeDto, Recipe>();
        CreateMap<UpdateStepDto, Step>();
        
        CreateMap<RegistrationApplicationUserDto, ApplicationUser>();
    }
}