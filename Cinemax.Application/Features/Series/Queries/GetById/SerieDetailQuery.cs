using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Series.Queries.GetById
{
    public class SerieDetailQuery
    {
        public class SerieDetailQueryRequest : IRequest<MessageResult<SeriesDto>>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<SerieDetailQueryRequest, MessageResult<SeriesDto>>
        {
            private readonly ISeriesRepository _seriesRepository;

            public Manejador(ISeriesRepository seriesRepository)
            {
                _seriesRepository = seriesRepository;
            }
            public async Task<MessageResult<SeriesDto>> Handle(SerieDetailQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _seriesRepository.GetSerieId(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? System.Net.HttpStatusCode.NotFound
                            : System.Net.HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<SeriesDto>.Of(message, result);
            }
        }
    }
}
