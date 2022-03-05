using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class FillInCodeQuestion : Question
    {
        [Required]
        public string CodeDescription { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public int FillCount { get; set; }

        [Required]
        public ICollection<FillInCodeBlock> FillInCodeBlocks { get; set; }


        public FillInCodeQuestion()
        {
            this.QuestionType = Models.QuestionType.FILL_IN_CODE;
        }

        
    }
}