using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class UserReadDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}