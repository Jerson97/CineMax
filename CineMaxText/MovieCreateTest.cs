using Xunit;
using Moq;
using AutoMapper;
using System.Net;
using Cinemax.Application.Features.Movies.Commands.Create;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Result;
using CineMax.Domain.Models;

public class MovieCreateTest
{
    private readonly Mock<IMovieRepository> _movieRepoMock;
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly Mock<IMapper> _mapperMock;

    public MovieCreateTest()
    {
        _movieRepoMock = new Mock<IMovieRepository>();
        _dbContextMock = new Mock<IApplicationDbContext>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task Handle_ShouldReturnMessageResult_WhenMovieIsCreated()
    {
        // Arrange
        var request = new MovieCreate.MovieCreateRequest
        {
            Title = "Matrix",
            Description = "Sci-fi",
            ReleaseDate = new DateTime(1999, 3, 31),
            Duration = 120,
            CategoryIds = new List<int> { 1 }
        };

        _movieRepoMock
            .Setup(r => r.InsertMovie(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ServiceStatus.Ok, 8, "Película creada exitosamente"));

        var handler = new MovieCreate.Manejador(_dbContextMock.Object, _mapperMock.Object, _movieRepoMock.Object);

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.Equal("Película creada exitosamente", result.Message);
        Assert.Equal(8, result.Data);
        Assert.Equal(1, result.Code); // por tu MessageResult.Of
    }

    [Fact]
    public async Task Handle_ShouldThrowErrorHandler_WhenServiceStatusIsNotFound()
    {
        // Arrange
        var request = new MovieCreate.MovieCreateRequest { Title = "Inexistente" };

        _movieRepoMock
            .Setup(r => r.InsertMovie(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ServiceStatus.NotFound, null, "Categoría no encontrada"));

        var handler = new MovieCreate.Manejador(_dbContextMock.Object, _mapperMock.Object, _movieRepoMock.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ErrorHandler>(() => handler.Handle(request, default));
        Assert.Equal(HttpStatusCode.NotFound, ex.Code);
        Assert.Equal("Categoría no encontrada", ex.Message);
    }
}
