using AutoMapper;
using Cinemax.Application.DTOs;
using CineMax.Domain.Entities;
using static Cinemax.Application.Features.Actors.Command.Create.ActorCreate;
using static Cinemax.Application.Features.Category.Command.Create.CategoryCreate;
using static Cinemax.Application.Features.Directors.Command.Create.DirectorCreate;
using static Cinemax.Application.Features.Episodes.Command.Create.EpisodeCreate;
using static Cinemax.Application.Features.Movies.Commands.Create.MovieCreate;
using static Cinemax.Application.Features.Seasons.Command.Create.SeasonCreate;
using static Cinemax.Application.Features.Series.Commands.Create.SeriesCreate;

namespace Cinemax.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<MovieCreateRequest, Movie>();
            CreateMap<Movie, MovieDto>();

            CreateMap<Movie, MovieByIdDto>()
            .ForMember(x => x.CategoryList, y => y.MapFrom(z => z.MovieCategories.Select(a => a.Category).ToList()))
            .ForMember(x => x.ActorList, y => y.MapFrom(z => z.MovieActor.Select(a => a.Actor).ToList()))
            .ForMember(x => x.DirectorList, y => y.MapFrom(z => z.MovieDirectors.Select(a => a.Director).ToList()));

            CreateMap<Series, SeriesDto>();

            CreateMap<Series, SerieByIdDto>()
                .ForMember(x => x.CategoryList, y => y.MapFrom(z => z.SeriesCategories.Select(a => a.Category).ToList()))
                .ForMember(x => x.ActorList, y => y.MapFrom(z => z.SeriesActor.Select(a => a.Actor).ToList()))
                .ForMember(x => x.DirectorList, y => y.MapFrom(z => z.SeriesDirectors.Select(a => a.Director).ToList()))
                .ForMember(x => x.SeasonList, y => y.MapFrom(z => z.Seasons));

            CreateMap<SeasonCreateRequest, Season>();
            CreateMap<EpisodeCreateRequest,  Episode>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryCreateRequest, Category>();
            CreateMap<Director, DirectorDto>();
            CreateMap<Actor, ActorDto>();
            CreateMap<DirectorCreateRequest, Director>();
            CreateMap<ActorCreateRequest, Actor>();
            CreateMap<SeriesCreateRequest, Series>();
            CreateMap<Season, SeasonDto>();
            CreateMap<Episode, EpisodeDto>();
            CreateMap<Movie, SearchByNameDto>();
            CreateMap<Series, SearchByNameDto>();
        }
    }
}
