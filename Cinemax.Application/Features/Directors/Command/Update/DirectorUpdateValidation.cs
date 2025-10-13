using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using static Cinemax.Application.Features.Directors.Command.Update.DirectorUpdate;

namespace Cinemax.Application.Features.Directors.Command.Update
{
    public class DirectorUpdateValidation : AbstractValidator<DirectorUpdateRequest>
    {
        public DirectorUpdateValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatario.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
        }
    }
}
