using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using static Cinemax.Application.Features.Auth.Command.Login;

namespace Cinemax.Application.Features.Auth.Command
{
    public class LoginValidation : AbstractValidator<LoginRequest>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("El valor no puede ser null");
            RuleFor(x => x.Password).NotEmpty().WithMessage("El valor no puede ser null");
        }
    }
}
