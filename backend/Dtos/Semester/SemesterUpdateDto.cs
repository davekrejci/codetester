using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Codetester.Models;

namespace Codetester.Dtos
{
    public class SemesterUpdateDto
    {
        [Required]
        public string Year { get; set; }
        [Required]
        public string SemesterType { get; set; }
        [Required]
        public Course Course { get; set; }
        public ICollection<User> EnrolledStudents { get; set; }
    }
}