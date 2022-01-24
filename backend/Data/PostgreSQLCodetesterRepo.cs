using System;
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

        public void CreateQuestion(Question question)
        {
            if(question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _context.Questions.Add(question);
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            return _context.Questions.ToList();
        }

        public Question GetQuestionById(int id)
        {
            return _context.Questions.FirstOrDefault(q => q.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateQuestion(Question question)
        {
            // Nothing, updated automatically in controller thanks to mapping from DTO to repo model
            // Keep here for sake of Interface implementation and/or possible future implementation changes
        }
    }
}