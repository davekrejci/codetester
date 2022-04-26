using System.Collections.Generic;

namespace Codetester.Dtos
{
    public class FillInCodeQuestionInstanceAttemptTurnInDto : QuestionInstanceAttemptTurnInDto
    {
        public List<FillInCodeBlockInstanceAttemptDto> FillInCodeBlocks { get; set; }

    }
}