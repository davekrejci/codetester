using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
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