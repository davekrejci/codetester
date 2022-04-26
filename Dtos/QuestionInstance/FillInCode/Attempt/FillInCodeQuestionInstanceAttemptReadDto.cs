using System.Collections.Generic;

namespace Codetester.Dtos
{
    public class FillInCodeQuestionInstanceAttemptReadDto : QuestionInstanceAttemptReadDto
    {
        public string CodeDescription { get; set; }
        public string Code { get; set; }
        public ICollection<FillInCodeBlockInstanceAttemptDto> FillInCodeBlocks { get; set; }

    }
}