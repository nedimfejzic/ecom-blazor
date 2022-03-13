using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Shared
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 5)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage ="Passwords do not match!")]
        public string PasswordConfirm { get; set; } = string.Empty;


    }
}
