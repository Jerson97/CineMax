using AutoMapper;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Actors.Command.Create.ActorCreate;
using static Cinemax.Application.Features.Actors.Command.Update.ActorUpdate;
using static Cinemax.Application.Features.Actors.Queries.GetAll.ActorQuery;

namespace Cinemax.Persistence.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActorRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(ServiceStatus, DataCollection<ActorDto>?, string)> GetActor(ActorQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var page = Math.Max(1, request.Page);
                var pageSize = request.Amount <= 0 ? 5
                               : request.Amount > 50 ? 50
                               : request.Amount;

                var skip = (page - 1) * pageSize;

                var query = await _context.Actors
                    .Skip(skip)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var total = await _context.Actors.CountAsync(cancellationToken);

                var directorDto = _mapper.Map<List<ActorDto>>(query);

                var directorDataCollection = new DataCollection<ActorDto>
                {
                    Items = directorDto,
                    Total = total,
                    Page = page,
                    Pages = (int)Math.Ceiling((double)total / pageSize)
                };

                return (ServiceStatus.Ok, directorDataCollection, "Actor obtenido exitosamente");
            }
            catch (Exception ex)
            {

                return (ServiceStatus.InternalError, null,
                    $"Error al Consultar -> {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> InsertActor(ActorCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = _mapper.Map<Actor>(request);

                await _context.Actors.AddAsync(actor, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, actor.Id, "Actor creado exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"Error al crear al Actor: {ex.Message}");
            }
        }

        public async Task<(ServiceStatus, int?, string)> UpdateActor(ActorUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var actor = await _context.Actors
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (actor == null)
                {
                    return (ServiceStatus.NotFound, null, "Actor no existe");
                }

                actor.Name = request.Name;

                await _context.SaveChangesAsync(cancellationToken);

                return (ServiceStatus.Ok, actor.Id, "Actor actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (ServiceStatus.InternalError, null, $"Error al actualizar actor: {ex.Message}");
            }
            
        }
    }
}
