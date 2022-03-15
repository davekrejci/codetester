using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Codetester.Dtos;

namespace Codetester.Dtos
{
    public class FillInCodeQuestionReadDto : QuestionReadDto
    {
        public string CodeDescription { get; set; }

        public string Code { get; set; }

        public int FillCount { get; set; }

        public ICollection<FillInCodeBlockReadDto> FillInCodeBlocks { get; set; }
        
    }
}