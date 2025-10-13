using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Directors.Command.Create.DirectorCreate;
using static Cinemax.Application.Features.Directors.Command.Update.DirectorUpdate;
using static Cinemax.Application.Features.Directors.Queries.GetAll.DirectorQuery;

namespace Cinemax.Persistence.Repositories
{
    public class DirectorRepository : IDirectoryRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DirectorRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<(ServiceStatus, DataCollection<DirectorDto>?, string)> GetDirector(DirectorQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var page = Math.Max(1, request.Page);
                var pageSize = Math.Max(1, request.Amount);

                var skip = (page - 1) * pageSize;

                var query = await _context.Directors
                    .Skip(skip)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var total = await _context.Directors.CountAsync(cancellationToken);

                var directorDto = _mapper.Map<List<DirectorDto>>(query);

                var directorDataCollection = new DataCollection<DirectorDto>
                {
                    Items = directorDto,
                    Total = total,
                    Page = page,
                    Pages = (int)Math.Ceiling((double)total / pageSize)
                };

                return (ServiceStatus.Ok, directorDataCollection, "Director obtenido exitosamente");
            }
            catch (Exception ex)
            {

                return (ServiceStatus.InternalError, null,
                    $"Error al Consultar -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> InsertDirector(DirectorCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var director = _mapper.Map<Director>(request);

                await _context.Directors.AddAsync(director, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, director.Id, "Director creado exitosamente");
            }
            catch (Exception ex)
            {

                return (ServiceStatus.InternalError, null, $"Error al crear categoria: {ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> UpdateDirector(DirectorUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var director = await _context.Directors
                    .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                if (director == null)
                {
                    return (ServiceStatus.NotFound, null, "Director no encontrado");
                }

                director.Name = request.Name ?? director.Name;

                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, director.Id, "Director actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"Error al actualizar categoria: {ex.Message}");
            }
        }
    }
}
