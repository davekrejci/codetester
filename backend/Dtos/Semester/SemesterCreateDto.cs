using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class SemesterCreateDto
    {
        [Required]
        public string Year { get; set; }
        [Required]
        public string SemesterType { get; set; }
        [Required]
        public string CourseCode { get; set; }
    }
}