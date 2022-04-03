using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class MultiChoiceQuestionInstance : QuestionInstance
    {
        [Required]
        public string QuestionText { get; set; }

        [Required]
        public List<MultiChoiceAnswer> Answers { get; set; }

        public int SelectedAnswer { get; set; }


        public MultiChoiceQuestionInstance()
        {
            this.QuestionType = Models.QuestionType.MULTI_CHOICE;
        }
    }
}