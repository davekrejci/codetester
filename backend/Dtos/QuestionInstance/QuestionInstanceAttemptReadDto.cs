using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public abstract class QuestionInstanceAttemptReadDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string QuestionType { get; set; }
    }
}