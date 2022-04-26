using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class MultiChoiceAnswerAttemptReadDto
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public bool IsSelected { get; set; }
    }
}