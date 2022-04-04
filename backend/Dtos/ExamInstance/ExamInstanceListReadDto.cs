using Newtonsoft.Json;

namespace Codetester.Dtos
{
    public class ExamInstanceListReadDto
    {
        [JsonConverter(typeof(HashIdJsonConverter))]
        public int Id { get; set; }

        public ExamReadMinimalDto Exam { get; set; }
        
    }
}