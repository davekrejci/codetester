using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Codetester.Models;

namespace Codetester.Dtos
{
    public class MultiChoiceQuestionInstanceResultDto : QuestionInstanceResultDto
    {
        public string QuestionText { get; set; }
        public List<MultiChoiceAnswerResultDto> Answers { get; set; }

    }
}