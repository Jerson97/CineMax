using FluentValidation;
using static Cinemax.Application.Features.Movies.Commands.Update.MovieUpdate;

namespace Cinemax.Application.Features.Movies.Commands.Update
{
    public class MovieUpdateValidation : AbstractValidator<MovieUpdateRequest>
    {
        public MovieUpdateValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID de la película debe ser mayor que 0.");
            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage("El titulo no puede exceder los 100 caracteres.");
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("La descripción no debe exceder los 1000 caracteres.");
            RuleFor(x => x.Duration)
                .GreaterThan(0).When(x => x.Duration > 0)
                .WithMessage("La duración debe ser mayor a 0.");
            RuleFor(x => x.CategoryIds)
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
