using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Codetester.Dtos;

namespace Codetester.Models
{
    public class FillInCodeQuestionInstance : QuestionInstance
    {
        [Required]
        public string CodeDescription { get; set; }

        [Required]
        public string Code { get; set; }

        public ICollection<FillInCodeBlockInstance> FillInCodeBlocks { get; set; }


        public FillInCodeQuestionInstance()
        {
            this.QuestionType = Models.QuestionType.FILL_IN_CODE;
        }

        public override void EvaluateQuestionAttempt(QuestionInstanceAttemptTurnInDto questionInstanceAttempt)
        {
            int score = 0;
            bool hasACorrectAnswer = false;
            FillInCodeQuestionInstanceAttemptTurnInDto attempt = questionInstanceAttempt as FillInCodeQuestionInstanceAttemptTurnInDto;
            foreach (FillInCodeBlockInstance block in this.FillInCodeBlocks)
            {
                var attemptBlock = attempt.FillInCodeBlocks.FirstOrDefault(b => b.WidgetId == block.WidgetId);
                block.Answer = attemptBlock.Answer; 
                if (block.Content == block.Answer)
                {
                    score++;
                    hasACorrectAnswer = true;
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