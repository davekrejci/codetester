using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class FillInCodeBlockCreateDto
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int StartPosition { get; set; }
        [Required]
        public int EndPosition { get; set; }

    }
}