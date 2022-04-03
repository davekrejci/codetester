using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class FillInCodeQuestionInstance : QuestionInstance
    {
        [Required]
        public string CodeDescription { get; set; }

        [Required]
        public string Code { get; set; }

        public string[] Answers { get; set; }


        public FillInCodeQuestionInstance()
        {
            this.QuestionType = Models.QuestionType.FILL_IN_CODE;
        }

    }
}