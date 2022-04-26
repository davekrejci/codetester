using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Codetester.Dtos;

namespace Codetester.Models
{
    public class MultiChoiceQuestionInstance : QuestionInstance
    {
        [Required]
        public string QuestionText { get; set; }

        [Required]
        public ICollection<MultiChoiceAnswerInstance> Answers { get; set; }

        public MultiChoiceQuestionInstance()
        {
            this.QuestionType = Models.QuestionType.MULTI_CHOICE;
        }

        public override void EvaluateQuestionAttempt(QuestionInstanceAttemptTurnInDto questionInstanceAttempt)
        {
            int score = 0;
            bool hasACorrectAnswer = false;
            MultiChoiceQuestionInstanceAttemptTurnInDto attempt = questionInstanceAttempt as MultiChoiceQuestionInstanceAttemptTurnInDto;
            foreach (MultiChoiceAnswerInstance answer in this.Answers)
            {
                var attemptAnswer = attempt.Answers.FirstOrDefault(a => a.Id == answer.Id);
                answer.IsSelected = attemptAnswer.isSelected;
                if (answer.IsSelected && answer.IsCorrect)
                {
                    score++;
                    hasACorrectAnswer = true;
                }
                if(answer.IsSelected && !answer.IsCorrect)
                {
                    score--;
                }
            }

            if (score == this.MaxScore)
            {
                this.State = QuestionInstanceState.CORRECT;
            }
            else if (score < this.MaxScore && hasACorrectAnswer)
            {
                this.State = QuestionInstanceState.PARTIALLYCORRECT;
            }
            else
            {
                this.State = QuestionInstanceState.INCORRECT;
            }
            this.Score = score;
        }
    }
}