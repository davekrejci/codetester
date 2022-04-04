using System.Collections.Generic;
using Newtonsoft.Json;

namespace Codetester.Dtos
{
    public class ExamInstanceAttemptReadDto
    {
        [JsonConverter(typeof(HashIdJsonConverter))]
        public int Id { get; set; }

        public ExamReadMinimalDto Exam { get; set; }
        
        public ICollection<QuestionInstanceAttemptReadDto> QuestionInstances { get; set; }

    }
}