using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class MultiChoiceQuestion : Question
    {
        [Required]
        public string QuestionText { get; set; }

        [Required]
        public List<MultiChoiceAnswer> Answers { get; set; }


        public MultiChoiceQuestion()
        {
            this.QuestionType = Models.QuestionType.MULTI_CHOICE;
        }

    }
}