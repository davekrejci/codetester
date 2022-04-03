using System;
using System.Collections.Generic;
using Codetester.Models;

namespace Codetester.Dtos
{
    public class ExamReadMinimalDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public SemesterReadMinimalDto Semester { get; set; }
        
    }
}