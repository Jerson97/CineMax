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
                .ForMember(x => x.CategoryList, y => y.MapFrom(z => z.MovieCategories.Select(a => a.Category).ToList()))
                .ForMember(x => x.ActorList, y => y.MapFrom(z => z.MovieActor.Select(a => a.Actor).ToList()))
                .ForMember(x => x.DirectorList, y => y.MapFrom(z => z.MovieDirectors.Select(a => a.Director).ToList()));
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryCreateRequest, Category>();
            CreateMap<Director, DirectorDto>();
            CreateMap<Actor, ActorDto>();
        }
    }
}
