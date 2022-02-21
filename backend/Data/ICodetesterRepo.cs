using System.Collections.Generic;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Data
{
    public interface ICodetesterRepo
    {
        bool SaveChanges();
        IEnumerable<Question> GetAllQuestions();
        Question GetQuestionById(int id);
        void CreateQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteQuestion(Question question);
    }
}