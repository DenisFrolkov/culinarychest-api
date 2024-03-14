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
            user => user.Login,
            opt => opt.MapFrom(au => string.Join(' ', au.Login))
        );
        CreateMap<Recipe, RecipeDto>().ForMember(
            recipe => recipe.Title,
            opt => opt.MapFrom(r => string.Join(' ', r.Title))
        );
    }
}