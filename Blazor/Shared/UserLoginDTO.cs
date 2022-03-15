using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Shared
{
    public class UserLoginDTO
    {
        [Required, EmailAddress, StringLength(200, MinimumLength = 5)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 5)]
        public string Password { get; set; } = string.Empty;
    }
}
