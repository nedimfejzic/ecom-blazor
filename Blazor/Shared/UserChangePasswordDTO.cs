using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Shared
{
    public class UserChangePasswordDTO
    {
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password", ErrorMessage ="Password do not match!")]
        public string PasswordConfirm { get; set; } = string.Empty;
    }
}
