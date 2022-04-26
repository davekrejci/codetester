
namespace Codetester.Dtos
{
    public abstract class QuestionInstanceAttemptReadDto
    {
        public int Id { get; set; }
        public string QuestionType { get; set; }
        public bool isAnswered { get; set; }
    }
}