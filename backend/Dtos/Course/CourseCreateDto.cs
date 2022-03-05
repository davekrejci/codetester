using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class CourseCreateDto
    {
        [Required]
        public string CourseCode { get; set; }
        [Required]
        public string CourseName { get; set; }
    }
}