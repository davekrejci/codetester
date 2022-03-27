using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class UserLoginReadDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}