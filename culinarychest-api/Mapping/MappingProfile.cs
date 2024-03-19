using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace culinarychest_api.Mapping;

public class MappingProfile : Profile
{
    // Класс MappingProfile используется для настройки AutoMapper, библиотеки, которая автоматизирует процесс
    // преобразования объектов одного типа в объекты другого типа.
    public MappingProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserDto>().ForMember(
            applicationUser => applicationUser.UserId,
            opt => opt.MapFrom(au => string.Join(' ', au.UserId))
        );
        
        CreateMap<FavoriteRecipe, FavoriteRecipeDto>().ForMember(
            favoriteRecipe => favoriteRecipe.FavoriteRecipeId,
            opt => opt.MapFrom(fR => string.Join(' ', fR.FavoriteRecipeId))
        );
        
        CreateMap<Recipe, RecipeDto>();
        
        CreateMap<Step, StepDto>().ForMember(
            step => step.StepId,
            opt => opt.MapFrom(s => string.Join(' ', s.StepId))
        );
    }
}