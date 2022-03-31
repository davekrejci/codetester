using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class UserAdminPasswordResetDto
    {
        [Required]
        public string Password { get; set; }
    }
}