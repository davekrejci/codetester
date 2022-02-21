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

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateQuestion(Question question)
        {
            // Nothing, updated automatically in controller thanks to mapping from DTO to repo model
            // Keep here for sake of Interface implementation and/or possible future implementation changes

            // Update Tags

            // Check question type

            // Deal with Multi-Choice field

            // Deal with Fill-In-Code fields
            // foreach (var block in questionUpdateDto.FillInCodeBlocks)
            // {
            //     var blockModel = _context.FillInCodeBlocks.FirstOrDefault(b => b.Id == block.Id);
            //     if(blockModel == null)
            //     {
            //         questionModel.
            //     }
            // }

            
        }

        // public void UpdateFillInCodeQuestion(FillInCodeQuestion question, QuestionUpdateDto questionUpdateDTO)
        // {
        //     // Nothing, updated automatically in controller thanks to mapping from DTO to repo model
        //     // Keep here for sake of Interface implementation and/or possible future implementation changes
        //     _context.Entry(question).CurrentValues.SetValues(questionUpdateDTO);
            
        //     //remove deleted fillInCodeBlocks
        //     question.FillInCodeBlocks
        //     .Where(fillInCodeBlock => !questionUpdateDTO.FillInCodeBlocks.Any(fillInCodeBlockDto => fillInCodeBlockDto.Id == fillInCodeBlock.Id))
        //     .Each(deleted => ctx.DetailSet.Remove(deleted));

        //     //update or add details
        //     questionUpdateDTO.FillInCodeBlocks.Each(FillInCodeBlockCreateDto =>
        //     {
        //         var detail = order.Details.FirstOrDefault(d => d.Id == detailDTO.Id);
        //         if (detail == null)
        //         {
        //             detail = new Detail();
        //             order.Details.Add(detail);
        //         }
        //         detail.Name = detailDTO.Name;
        //         detail.Quantity = detailDTO.Quantity;
        //     });
        // }
    }
}