using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Codetester.Models;

namespace Codetester.Dtos
{
    public class SemesterReadMinimalDto
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string SemesterType { get; set; }
        public CourseReadMinimalDto Course { get; set; }

    }
}