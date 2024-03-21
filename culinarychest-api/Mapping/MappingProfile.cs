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

        CreateMap<ApplicationUserForCreationDto, ApplicationUser>();
        CreateMap<FavoriteRecipeForCreationDto, FavoriteRecipe>();
        CreateMap<RecipeForCreationDto, Recipe>();
        CreateMap<StepForCreationDto, Step>();
    }
}