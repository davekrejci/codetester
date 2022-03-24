using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class UserCreateDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

    }
}