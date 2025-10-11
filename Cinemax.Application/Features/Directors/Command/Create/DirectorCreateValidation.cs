using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using static Cinemax.Application.Features.Directors.Command.Create.DirectorCreate;

namespace Cinemax.Application.Features.Directors.Command.Create
{
    public class DirectorCreateValidation : AbstractValidator<DirectorCreateRequest>
    {
        public DirectorCreateValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatario.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
        }
    }
}
