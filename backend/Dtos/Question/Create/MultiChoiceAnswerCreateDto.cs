using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class MultiChoiceAnswerCreateDto
    {
        public int Id { get; set; }

        [Required]
        public string AnswerText { get; set; }
        
        [Required]
        public bool IsCorrect { get; set; }

    }
}