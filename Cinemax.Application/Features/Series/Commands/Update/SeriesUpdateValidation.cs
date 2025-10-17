using FluentValidation;
using static Cinemax.Application.Features.Series.Commands.Update.SeriesUpdate;

namespace Cinemax.Application.Features.Series.Commands.Update
{
    public class SeriesUpdateValidation : AbstractValidator<SeriesUpdateRequest>
    {
        public SeriesUpdateValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID de la película debe ser mayor que 0.");
            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage("El titulo no puede exceder los 100 caracteres.");
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("La descripción no debe exceder los 1000 caracteres.");
            RuleFor(x => x.CategoryIds)
                .Must(ids => ids.All(id => id > 0)).WithMessage("Todos los ID de categoría deben ser mayores que 0.");
            RuleFor(x => x.DirectorIds)
                .Must(ids => ids.All(id => id > 0)).WithMessage("Todos los ID de director deben ser mayores que 0.");
            RuleFor(x => x.ActorIds)
                .Must(ids => ids.All(id => id > 0)).WithMessage("Todos los ID de actor deben ser mayores que 0.");
        }
    }
}
