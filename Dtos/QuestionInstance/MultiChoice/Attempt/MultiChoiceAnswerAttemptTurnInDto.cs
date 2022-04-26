using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class MultiChoiceAnswerAttemptTurnInDto
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public bool isSelected { get; set; }
        
    }
}