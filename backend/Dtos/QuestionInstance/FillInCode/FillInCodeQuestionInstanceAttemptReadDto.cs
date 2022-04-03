using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class FillInCodeQuestionInstanceAttemptReadDto : QuestionInstanceAttemptReadDto
    {
        public string CodeDescription { get; set; }
        public string Code { get; set; }

    }
}