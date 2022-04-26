using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Codetester.Models;

namespace Codetester.Dtos
{
    public class QuestionUpdateDto
    {
        // QUESTION
        public ICollection<TagCreateDto> Tags { get; set; }

        [Required]
        public string QuestionType { get; set; }
        
        // MULTICHOICE
        [RequiredIf("QuestionType", Models.QuestionType.MULTI_CHOICE)]
        public string QuestionText { get; set; }
        [RequiredIf("QuestionType", Models.QuestionType.MULTI_CHOICE)]
        public List<MultiChoiceAnswerCreateDto> Answers { get; set; }

        // FILLINCODE
        [RequiredIf("QuestionType", Models.QuestionType.FILL_IN_CODE)]
        public string CodeDescription { get; set; }

        [RequiredIf("QuestionType", Models.QuestionType.FILL_IN_CODE)]
        public string Code { get; set; }
        [RequiredIf("QuestionType", Models.QuestionType.FILL_IN_CODE)]
        public int? FillCount { get; set; }
        [RequiredIf("QuestionType", Models.QuestionType.FILL_IN_CODE)]
        public ICollection<FillInCodeBlockCreateDto> FillInCodeBlocks { get; set; }

    }
}