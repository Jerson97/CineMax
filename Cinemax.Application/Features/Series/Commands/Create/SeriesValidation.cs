using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using static Cinemax.Application.Features.Series.Commands.Create.SeriesCreate;

namespace Cinemax.Application.Features.Series.Commands.Create
{
    public class SeriesValidation : AbstractValidator<SeriesCreateRequest>
    {
        public SeriesValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El titulo es obligatario.")
                .MaximumLength(100).WithMessage("El titulo no puede exceder los 100 caracteres.");
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("La descripción no debe exceder los 1000 caracteres.");
            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("La fecha de lanzamiento es requerida.");
            RuleFor(x => x.Duration)
                .GreaterThan(0).When(x => x.Duration > 0)
                .WithMessage("La duración debe ser mayor a 0.");
            RuleFor(x => x.CategoryIds)
                .NotEmpty().WithMessage("Se requiere al menos una categoria")
                .Must(ids => ids.All(id => id > 0)).WithMessage("Todos los ID de categoría deben ser mayores que 0.");
            RuleFor(x => x.DirectorIds)
                .NotEmpty().WithMessage("Se requiere al menos un director")
                .Must(ids => ids.All(id => id > 0)).WithMessage("Todos los ID de director deben ser mayores que 0.");
            RuleFor(x => x.ActorIds)
                .NotEmpty().WithMessage("Se requiere al menos un actor")
                .Must(ids => ids.All(id => id > 0)).WithMessage("Todos los ID de actor deben ser mayores que 0.");
        }
    }
}
