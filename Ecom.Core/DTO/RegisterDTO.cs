using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTO
{
    public record LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

    public record RegisterDTO: LoginDTO
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

    }

    public record ResetPasswordDTO : LoginDTO
    {
        public string Token { get; set; }
    }

    public record ActiveEmailDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
