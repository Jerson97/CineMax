using FluentValidation;
using static Cinemax.Application.Features.Actors.Command.Update.ActorUpdate;

namespace Cinemax.Application.Features.Actors.Command.Update
{
    public class ActorUpdateValidation : AbstractValidator<ActorUpdateRequest>
    {
        public ActorUpdateValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatario.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
        }
    }
}
