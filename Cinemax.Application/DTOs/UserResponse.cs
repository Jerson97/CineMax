using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemax.Application.DTOs
{
    public class UserResponse
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Token { get; set; }
        public string? Email { get; set; }
    }
}
