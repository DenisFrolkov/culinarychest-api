using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Newtonsoft.Json;

namespace culinarychest_api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FavoriteRecipe, FavoriteRecipeDto>();
        CreateMap<Recipe, RecipeDto>();
        CreateMap<Step, StepDto>();

        CreateMap<CreateFavoriteRecipeDto, FavoriteRecipe>();
        CreateMap<CreateRecipeDto, Recipe>();
        CreateMap<CreateStepsDto, Step>();

        CreateMap<UpdateRecipeDto, Recipe>();
        CreateMap<UpdateStepDto, Step>();
        
        CreateMap<RegistrationApplicationUserDto, ApplicationUser>();
    }
}