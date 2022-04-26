using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            var instanceAnswers = new List<MultiChoiceAnswerInstance>();
            foreach (MultiChoiceAnswer answer in this.Answers)
            {
                MultiChoiceAnswerInstance instanceAnswer = new MultiChoiceAnswerInstance(answer);
                instanceAnswers.Add(instanceAnswer);
            }
            instance.Answers = instanceAnswers;
            instance.MaxScore = instanceAnswers.Where(a => a.IsCorrect == true).Count();
            return instance;
        }
    }
}