using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cinemax.Application.Common;
using Cinemax.Application.DTOs;
using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using CineMax.Domain.Result;
using MediatR;

namespace Cinemax.Application.Features.Category.Command.Create
{
    public class CategoryCreate
    {
        public class CategoryCreateRequest : IRequest<MessageResult<int>>
        {
            public string? Name { get; set; } 
        }
        public class Manejador : IRequestHandler<CategoryCreateRequest, MessageResult<int>>
        {
            private readonly ICategoryRepository _categoryRepository;

            public Manejador(ICategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }
            public async Task<MessageResult<int>> Handle(CategoryCreateRequest request, CancellationToken cancellationToken)
            {
                var (status, categoryId, message) = await _categoryRepository.InsertCategory(request, cancellationToken);

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
