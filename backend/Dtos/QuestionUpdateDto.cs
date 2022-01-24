using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class QuestionUpdateDto 
    {
        [Required]
        public string QuestionText { get; set; }
        [Required]
        public string QuestionType { get; set; }

    }
}