using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class MultiChoiceAnswer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AnswerText { get; set; }
        
        [Required]
        public bool IsCorrect { get; set; }

        //Foreign key for MultiChoiceQuestion
        public int MultiChoiceQuestionId { get; set; }
        public MultiChoiceQuestion multiChoiceQuestion { get; set; }

    }
}