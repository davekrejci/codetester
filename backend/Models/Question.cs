using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string QuestionText { get; set; }
        
        [Required]
        public string QuestionType { get; set; }

    }
}