using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class UserLoginDto
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
}