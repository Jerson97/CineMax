using AutoMapper;
using Cinemax.Application.DTOs;
using CineMax.Domain.Entities;
using static Cinemax.Application.Features.Movies.Commands.Create.MovieCreate;

namespace Cinemax.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<MovieCreateRequest, Movie>();

            CreateMap<Movie, MovieDto>()
            .ForMember(dest => dest.CategoryList, opt =>
            opt.MapFrom(src => src.MovieCategories.Select(mc => mc.CategoryId).ToList()));



        }
    }
}
