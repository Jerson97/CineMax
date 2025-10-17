using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using static Cinemax.Application.Features.Episodes.Command.Create.EpisodeCreate;

namespace Cinemax.Application.Features.Episodes.Command.Create
{
    public class EpisodeCreateValidation : AbstractValidator<EpisodeCreateRequest>
    {
        public EpisodeCreateValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El titulo es obligatario.")
                .MaximumLength(100).WithMessage("El titulo no puede exceder los 100 caracteres.");
            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage("El número de episodio debe ser mayor que 0.");

            RuleFor(x => x.SeasonId)
                .NotEmpty().WithMessage("El seasonId es obligatario.");
        }
    }
}
