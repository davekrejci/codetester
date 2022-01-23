using System.Collections.Generic;
using System.Linq;
using Codetester.Models;

namespace Codetester.Data
{
    public class PostgreSQLCodetesterRepo : ICodetesterRepo
    {
        private readonly CodetesterContext _context;

        public PostgreSQLCodetesterRepo(CodetesterContext context)
        {
            _context = context;
        }
        public IEnumerable<Question> GetAllQuestions()
        {
            return _context.Questions.ToList();
        }

        public Question GetQuestionById(int id)
        {
            return _context.Questions.FirstOrDefault(q => q.Id == id);
        }
    }
}