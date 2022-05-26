using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Codetester.Models
{
    [Index(propertyNames: nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }
        
        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        public ICollection<Semester> EnrolledSemesters { get; set; } 

        public ICollection<Course> Courses { get; set; } 

    }

    public static class UserRole
    {
        public const string ADMIN = "Admin";
        public const string TEACHER = "Teacher";
        public const string STUDENT = "Student";


    }

}