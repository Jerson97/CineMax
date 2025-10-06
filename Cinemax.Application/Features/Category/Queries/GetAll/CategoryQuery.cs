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

namespace Cinemax.Application.Features.Category.Queries.GetAll
{
    public class CategoryQuery
    {
        public class CategoryQueryRequest : IRequest<MessageResult<DataCollection<CategoryDto>>>
        {
            public int Page { get; set; } = 1;
            public int Amount { get; set; } = 5;
        }

        public class Manejador : IRequestHandler<CategoryQueryRequest, MessageResult<DataCollection<CategoryDto>>>
        {
            private readonly ICategoryRepository _categoryRepository;

            public Manejador(ICategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }
            public async Task<MessageResult<DataCollection<CategoryDto>>> Handle(CategoryQueryRequest request, CancellationToken cancellationToken)
            {
                var (status, result, message) = await _categoryRepository.GetCategory(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? System.Net.HttpStatusCode.NotFound
                            : System.Net.HttpStatusCode.InternalServerError,
                        message);

                return MessageResult<DataCollection<CategoryDto>>.Of(message, result);
            }
        }
    }
}
