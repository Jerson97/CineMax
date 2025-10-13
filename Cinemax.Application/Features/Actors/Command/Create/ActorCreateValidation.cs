using FluentValidation;
using static Cinemax.Application.Features.Actors.Command.Create.ActorCreate;

namespace Cinemax.Application.Features.Actors.Command.Create
{
    internal class ActorCreateValidation : AbstractValidator<ActorCreateRequest>
    {
        public ActorCreateValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatario.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
        }
    }
}
