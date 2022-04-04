using System;
using System.Collections.Generic;
using Codetester.Models;

namespace Codetester.Dtos
{
    public class ExamReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public SemesterReadMinimalDto Semester { get; set; }

        
        public ICollection<QuestionReadDto> Questions { get; set; }
        
        public ICollection<TagReadDto> Tags { get; set; }
        
    }
}