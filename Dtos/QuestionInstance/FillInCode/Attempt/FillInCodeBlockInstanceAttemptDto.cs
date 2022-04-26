using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class FillInCodeBlockInstanceAttemptDto
    {
        [Required]
        public string WidgetId { get; set; }
        public string Answer { get; set; }

    }
}