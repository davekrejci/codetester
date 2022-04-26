using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class MultiChoiceAnswerResultDto
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCorrect { get; set; }
    }
}