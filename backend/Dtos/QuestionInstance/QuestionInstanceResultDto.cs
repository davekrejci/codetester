
namespace Codetester.Dtos
{
    public abstract class QuestionInstanceResultDto
    {
        public int Id { get; set; }
        public string QuestionType { get; set; }
        public bool isAnswered { get; set; }
        public string State { get; set; }
        public int MaxScore { get; set; }
        public int Score { get; internal set; }

    }
}