using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Codetester.Dtos
{
    [JsonConverter(typeof(QuestionInstanceAttemptTurnInDtoJsonConverter))]
    public abstract class QuestionInstanceAttemptTurnInDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string QuestionType { get; set; }
    }
}