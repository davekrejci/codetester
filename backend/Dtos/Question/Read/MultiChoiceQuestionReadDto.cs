using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Codetester.Dtos;

namespace Codetester.Models
{
    public class MultiChoiceQuestionReadDto : QuestionReadDto
    {
        public string QuestionText { get; set; }

        public List<MultiChoiceAnswerReadDto> Answers { get; set; }

    }
}