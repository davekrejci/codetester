using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Dtos
{
    public class ExamUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        public int SemesterId { get; set; }
        
        public int[] QuestionIds { get; set; }
        
        public ICollection<TagCreateDto> Tags { get; set; }
        
    }
}