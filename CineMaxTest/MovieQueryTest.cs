using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Features.Movies.Queries.GetAll;
using Cinemax.Application.Interfaces;
using Moq;

namespace CineMaxTest
{
    public class MovieQueryTest
    {
        private readonly Mock<IMovieRepository> _movieRepoMock;
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;

        public MovieQueryTest()
        {
            _movieRepoMock = new Mock<IMovieRepository>();
            _dbContextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_ShouldReturnMessageResult_WhenMoviesAreFound()
        {
            // Arrange
            var request = new MovieQuery.MovieQueryRequest
            {
                Search = "Matrix",
                Page = 1,
                Amount = 5
            };
            var movieDataCollection = new DataCollection<MovieDto>
            {
                Items = new List<MovieDto>
                {
                    new MovieDto { Title = "Matrix", Description = "Sci-fi", ReleaseDate = new DateTime(1999, 3, 31), Duration = 120 }
                },
                Total = 1,
                Page = 1,
                Amount = 5
            };
            _movieRepoMock
                .Setup(r => r.GetMovie(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CineMax.Domain.Enum.ServiceStatus.Ok, movieDataCollection, "Películas encontradas exitosamente"));
            var handler = new MovieQuery.Manejador(_dbContextMock.Object, _mapperMock.Object, _movieRepoMock.Object);
            // Act
            var result = await handler.Handle(request, default);
            // Assert
            Assert.Equal("Películas encontradas exitosamente", result.Message);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data!.Items);
            Assert.Equal("Matrix", result.Data.Items[0].Title);
            Assert.Equal(1, result.Code); // por tu MessageResult.Of
        }

    }
}
