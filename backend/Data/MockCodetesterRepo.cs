using System.Collections.Generic;
using Codetester.Models;

namespace Codetester.Data
{
    public class MockCodetesterRepo : ICodetesterRepo
    {
        public IEnumerable<Question> GetAllQuestions()
        {
            var questions = new List<Question>
            {
                new Question{Id=0, QuestionType="test", QuestionText="What is the airspeed velocity of an unladen swallow?"},
                new Question{Id=1, QuestionType="test", QuestionText="To be or not to be?"},
                new Question{Id=2, QuestionType="test", QuestionText="If you work for a living, why do you kill yourself working?"}
            };
            return questions;
        }

        public Question GetQuestionById(int id)
        {
            return new Question{Id=0, QuestionType="test", QuestionText="What is the airspeed velocity of an unladen swallow?"};
        }
    }
}