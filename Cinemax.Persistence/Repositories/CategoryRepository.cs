using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Category.Command.Create.CategoryCreate;
using static Cinemax.Application.Features.Category.Command.Delete.CategoryDelete;
using static Cinemax.Application.Features.Category.Command.Update.CategoryUpdate;
using static Cinemax.Application.Features.Category.Queries.GetAll.CategoryQuery;

namespace Cinemax.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(ServiceStatus, int?, string)> DeleteCategory(CategoryDeleteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id);

                if (category == null)
                {
                    return (ServiceStatus.NotFound, null, "Categoria no encontrada");
                }

                if (!category.IsActive)
                {
                    return (ServiceStatus.FailedValidation, category.Id, "La categoria ya se elimino");
                }

                category.IsActive = false;

                await _context.SaveChangesAsync(cancellationToken);


                return (ServiceStatus.Ok, category.Id, "Categoria se elimino exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"\"Error al eliminar la categoria: {ex.Message}");
            }
        }

        public async Task<(ServiceStatus, DataCollection<CategoryDto>?, string)> GetCategory(CategoryQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var page = Math.Max(1, request.Page);
                var pageSize = request.Amount <= 0 ? 5
                               : request.Amount > 50 ? 50
                               : request.Amount; ;

                var skip = (page - 1) * pageSize;

                var query = _context.Categories.Where(x => x.IsActive == true);

                var total = await query.CountAsync(cancellationToken);

                var category = await query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                var categoryDto = _mapper.Map<List<CategoryDto>>(category);

                var categoryDataCollection = new DataCollection<CategoryDto>
                {
                    Items = categoryDto,
                    Total = total,
                    Page = page,
                    Pages = (int)Math.Ceiling((double)total / pageSize)
                };

                return (ServiceStatus.Ok, categoryDataCollection, "Categoria obtenida exitosamente");
            }
            catch (Exception ex)
            {

                return (ServiceStatus.InternalError, null,
                    $"Error al Consultar -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> InsertCategory(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var category = _mapper.Map<Category>(request);

                await _context.Categories.AddAsync(category, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, category.Id, "Category creada exitosamente");
            }
            catch (Exception ex)
            {

                return (ServiceStatus.InternalError, null, $"Error al crear categoria: {ex.Message}");
            }

        }

        public async Task<(ServiceStatus, int?, string)> UpdateCategory(CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                if (category == null)
                {
                    return (ServiceStatus.NotFound, null, "Categoria no encontrada");
                }

                category.Name = request.Name ?? category.Name;

                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, category.Id, "Categoria actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"Error al actualizar categoria: {ex.Message}");
            }
        }
    }
}
