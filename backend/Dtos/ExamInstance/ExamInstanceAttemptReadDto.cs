using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codetester.Models;
using Newtonsoft.Json;

namespace Codetester.Dtos
{
    public class ExamInstanceAttemptReadDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ExamReadMinimalDto Exam { get; set; }
        
        [Required]
        public ICollection<QuestionInstanceAttemptReadDto> QuestionInstances { get; set; }

    }
}