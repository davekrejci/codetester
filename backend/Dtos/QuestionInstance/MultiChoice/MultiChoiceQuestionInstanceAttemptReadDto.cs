using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Codetester.Models;

namespace Codetester.Dtos
{
    public class MultiChoiceQuestionInstanceAttemptReadDto : QuestionInstanceAttemptReadDto
    {
        public string QuestionText { get; set; }
        public List<MultiChoiceAnswerAttemptReadDto> Answers { get; set; }

    }
}