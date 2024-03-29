using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class FillInCodeBlockCreateDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int StartPosition { get; set; }
        [Required]
        public int EndPosition { get; set; }

    }
}