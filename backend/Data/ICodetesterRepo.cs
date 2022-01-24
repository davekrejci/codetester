using System.Collections.Generic;
using Codetester.Models;

namespace Codetester.Data
{
    public interface ICodetesterRepo
    {
        bool SaveChanges();
        IEnumerable<Question> GetAllQuestions();
        Question GetQuestionById(int id);
        void CreateQuestion(Question question);
    }
}