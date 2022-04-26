using System.Collections.Generic;
using Newtonsoft.Json;

namespace Codetester.Dtos
{
    public class ExamInstanceResultDto
    {
        [JsonConverter(typeof(HashIdJsonConverter))]
        public int Id { get; set; }

        public ExamReadMinimalDto Exam { get; set; }

        public int MaxScore { get; set; }

        public int Score { get; set; }
        
        public ICollection<QuestionInstanceResultDto> QuestionInstances { get; set; }

    }
}