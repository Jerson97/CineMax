using System.Net;
using Azure;
using Cinemax.Application.DTOs;
using Cinemax.Application.Features.Auth.Command;
using Cinemax.Application.Interfaces;
using Cinemax.Application.Interfaces.Token;
using CineMax.Domain.Entities;
using CineMax.Domain.Enum;
using CineMax.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Cinemax.Application.Features.Auth.Command.Login;
using static Cinemax.Application.Features.Auth.Command.Register;

namespace Cinemax.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IApplicationDbContext _context;

        public UserRepository(IApplicationDbContext context, IJwtGenerator jwtGenerator, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(ServiceStatus, UserResponse?, string)> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);
            if (user == null)
            {
                throw new ErrorHandler(HttpStatusCode.NotFound, "El email no existe");
            }
            if (!await _userManager.CheckPasswordAsync(user, request.Password!))
            {
                throw new ErrorHandler(HttpStatusCode.BadRequest, "Password incorrecto");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password!, false, false);
            if (!result.Succeeded)
            {
                throw new ErrorHandler(HttpStatusCode.BadRequest, "Credenciales incorrectas");
            }
            else
            {
                var token = await _jwtGenerator.CreateToken(user);
                var response = new UserResponse
                {
                    Name = user.Name,
                    LastName = user.LastName,
                    Token = token,
                    UserName = user.UserName,
                    Email = user.Email
                };

                return (ServiceStatus.Ok, response, "Inicio de sesión exitoso"); 
            }
        }

        public async Task<(ServiceStatus, UserResponse?, string)> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            var exists = await _userManager.Users.AnyAsync(x => x.Email == request.Email);

            if (exists)
            {
                return (ServiceStatus.BadRequest, null, "El email ya existe");
            }

            var existsUserName = await _userManager.Users.AnyAsync(x => x.UserName == request.UserName);
            if (existsUserName)
            {
                return (ServiceStatus.BadRequest, null, "El username ya existe");
            }

            var user = new User
            {
                Name = request.Name,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password!);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ErrorHandler(HttpStatusCode.BadRequest, errors);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "USER");

                var token = await _jwtGenerator.CreateToken(user); 

                var response = new UserResponse
                {
                    Name = user.Name,
                    LastName = user.LastName,
                    Token = token,
                    UserName = user.UserName,
                    Email = user.Email
                };

                return (ServiceStatus.Ok, response, "Registro exitoso");
            }
        }
    }
}
