using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Category.Command.Update
{
    public class CategoryUpdate
    {
        public class CategoryUpdateRequest : IRequest<MessageResult<int>>
        {
            [JsonIgnore]
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        public class Manejador : IRequestHandler<CategoryUpdateRequest, MessageResult<int>>
        {
            private readonly ICategoryRepository _categoryRepository;

            public Manejador(ICategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }
            public async Task<MessageResult<int>> Handle(CategoryUpdateRequest request, CancellationToken cancellationToken)
            {
                var (status, categoryId, message) = await _categoryRepository.UpdateCategory(request, cancellationToken);

                if (status != ServiceStatus.Ok)
                    throw new ErrorHandler(
                        status == ServiceStatus.NotFound
                            ? HttpStatusCode.NotFound
                            : HttpStatusCode.InternalServerError,
                    message);

                return MessageResult<int>.Of(message, categoryId!.Value);
            }
        }
    }
}
