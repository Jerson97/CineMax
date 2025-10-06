using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using CineMax.Domain.Enum;
using static Cinemax.Application.Features.Category.Command.Create.CategoryCreate;
using static Cinemax.Application.Features.Category.Command.Delete.CategoryDelete;
using static Cinemax.Application.Features.Category.Command.Update.CategoryUpdate;
using static Cinemax.Application.Features.Category.Queries.GetAll.CategoryQuery;


namespace Cinemax.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<(ServiceStatus, DataCollection<CategoryDto>, string)> GetCategory(CategoryQueryRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> InsertCategory(CategoryCreateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> UpdateCategory(CategoryUpdateRequest request, CancellationToken cancellationToken);
        Task<(ServiceStatus, int?, string)> DeleteCategory(CategoryDeleteRequest request, CancellationToken cancellationToken);
    }
}
