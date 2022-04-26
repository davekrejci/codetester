using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Codetester.Dtos
{
    public class ExamInstanceAttemptTurnInDto
    {
        [JsonConverter(typeof(HashIdJsonConverter))]
        public int Id { get; set; }
        public ICollection<QuestionInstanceAttemptTurnInDto> QuestionInstances { get; set; }

    }
}