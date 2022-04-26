using System.Collections.Generic;

namespace Codetester.Dtos
{
    public class MultiChoiceQuestionInstanceAttemptTurnInDto : QuestionInstanceAttemptTurnInDto
    {
        public List<MultiChoiceAnswerAttemptTurnInDto> Answers { get; set; }

    }
}