using System.Collections.Generic;
using Codetester.Models;

namespace Codetester.Data
{
    public interface ICodetesterRepo
    {
        IEnumerable<Question> GetAllQuestions();
        Question GetQuestionById(int id);
    }
}