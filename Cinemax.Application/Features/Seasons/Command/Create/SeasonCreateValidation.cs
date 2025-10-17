using FluentValidation;
using static Cinemax.Application.Features.Seasons.Command.Create.SeasonCreate;

namespace Cinemax.Application.Features.Seasons.Command.Create
{
    public class SeasonCreateValidation : AbstractValidator<SeasonCreateRequest>
    {
        public SeasonCreateValidation()
        {
            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("El numero de temporada es obligatario.");
            RuleFor(x => x.SeriesId)
                .NotEmpty().WithMessage("La serieId es obligatario.");
        }
    }
}
