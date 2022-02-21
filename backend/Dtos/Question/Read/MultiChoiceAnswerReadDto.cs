using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class MultiChoiceAnswerReadDto
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}