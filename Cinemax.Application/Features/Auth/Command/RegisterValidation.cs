using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using static Cinemax.Application.Features.Auth.Command.Register;

namespace Cinemax.Application.Features.Auth.Command
{
    public class RegisterValidation : AbstractValidator<RegisterRequest>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("La contraseña debe tener al menos 6 caracteres");
            RuleFor(x => x.UserName).NotEmpty();
        }
    }
}
