using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class FillInCodeBlock
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int StartPosition { get; set; }
        [Required]
        public int EndPosition { get; set; }

        //Foreign key for FillInCodeQuestion
        public int FillInCodeQuestionId { get; set; }
        public FillInCodeQuestion fillInCodeQuestion { get; set; }

    }
}