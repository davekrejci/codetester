using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class CourseCreateDto
    {
        [Required]
        public string CourseCode { get; set; }
        [Required]
        public string CourseName { get; set; }
    }
}