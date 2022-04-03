using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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

        public override QuestionInstance CreateInstance()
        {
            MultiChoiceQuestionInstance instance = new MultiChoiceQuestionInstance();
            instance.QuestionText = this.QuestionText;
            instance.Answers = this.Answers;
            return instance;
        }
    }
}