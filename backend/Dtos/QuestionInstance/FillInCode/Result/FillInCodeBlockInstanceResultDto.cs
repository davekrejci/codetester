using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class FillInCodeBlockInstanceResultDto
    {
        public string WidgetId { get; set; }
        public string Content { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }

    }
}