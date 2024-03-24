using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace culinarychest_api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserDto>().ForMember(
            applicationUser => applicationUser.UserId,
            opt => opt.MapFrom(au => string.Join(' ', au.UserId))
        );
        CreateMap<FavoriteRecipe, FavoriteRecipeDto>();
        CreateMap<Recipe, RecipeDto>();
        CreateMap<Step, StepDto>();

        CreateMap<CreateApplicationUserDto, ApplicationUser>();
        CreateMap<CreateFavoriteRecipeDto, FavoriteRecipe>();
        CreateMap<CreateRecipeDto, Recipe>();
        CreateMap<CreateStepsDto, Step>();

        CreateMap<UpdateApplicationUserDto, ApplicationUser>();
        CreateMap<UpdateRecipeDto, Recipe>();
        CreateMap<UpdateStepDto, Step>();
    }
}