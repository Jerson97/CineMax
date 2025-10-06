using AutoMapper;
using Cinemax.Application.DTOs;
using CineMax.Domain.Entities;
using static Cinemax.Application.Features.Category.Command.Create.CategoryCreate;
using static Cinemax.Application.Features.Movies.Commands.Create.MovieCreate;

namespace Cinemax.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<MovieCreateRequest, Movie>();
            CreateMap<Movie, MovieDto>()
                .ForMember(x => x.CategoryList, y => y.MapFrom(z => z.MovieCategories.Select(a => a.Category).ToList()));
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryCreateRequest, Category>();
        }
    }
}
