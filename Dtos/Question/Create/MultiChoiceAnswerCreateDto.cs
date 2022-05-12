using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class MultiChoiceAnswerCreateDto
    {
        [Required]
        public string AnswerText { get; set; }
        
        [Required]
        public bool IsCorrect { get; set; }
    }
}