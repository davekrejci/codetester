using System;
using System.Collections.Generic;
using System.Linq;
using Codetester.Dtos;
using Codetester.Models;
using Microsoft.EntityFrameworkCore;

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
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _context.Questions.Add(question);
        }

        public void DeleteQuestion(Question question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            _context.Questions.Remove(question);
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            return _context.Questions
                                .Include("Answers")
                                .Include("FillInCodeBlocks")
                                .Include("Tags")
                                .ToList();
        }


        public Question GetQuestionById(int id)
        {
            return _context.Questions
                            .Include("Answers")
                            .Include("FillInCodeBlocks")
                            .Include("Tags")
                            .FirstOrDefault(q => q.Id == id);

        }
        public void UpdateQuestion(Question question)
        {
            // Nothing, updated automatically in controller thanks to mapping from DTO to repo model
            // Keep here for sake of Interface implementation and/or possible future implementation changes
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return _context.Tags.ToList();
        }
        public Tag GetTagById(int id)
        {
            return _context.Tags.FirstOrDefault(t => t.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void AddTagToQuestion(int tagId, int questionId)
        {
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == tagId);
            Question question = _context.Questions.FirstOrDefault(q => q.Id == questionId);

            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            question.Tags.Add(tag);
        }

        public void CreateTag(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            _context.Tags.Add(tag);
        }
    }
}