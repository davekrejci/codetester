using System.Collections.Generic;

namespace Codetester.Dtos
{
    public class FillInCodeQuestionInstanceResultDto : QuestionInstanceResultDto
    {
        public string CodeDescription { get; set; }
        public string Code { get; set; }
        public ICollection<FillInCodeBlockInstanceResultDto> FillInCodeBlocks { get; set; }

    }
}