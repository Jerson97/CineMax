using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Category.Command.Delete
{
    public class CategoryDelete 
    {
        public class CategoryDeleteRequest : IRequest<MessageResult<int>>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<CategoryDeleteRequest, MessageResult<int>>
        {
            private readonly ICategoryRepository _categoryRepository;

            public Manejador(ICategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }
            public async Task<MessageResult<int>> Handle(CategoryDeleteRequest request, CancellationToken cancellationToken)
            {
                var (status, categoryId, message) = await _categoryRepository.DeleteCategory(request, cancellationToken);

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
