using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Codetester.Models
{
    public class ExamInstance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Exam Exam { get; set; }
        
        [Required]
        public ICollection<QuestionInstance> QuestionInstances { get; set; }

        [Required]
        public User User { get; set; }
        
        public ExamInstance()
        {
            // default
        }
        public ExamInstance(Exam exam, User user)
        {
            this.Exam = exam;
            this.User = user;
            this.QuestionInstances = new List<QuestionInstance>();
            GenerateQuestionInstances();
        }

        private void GenerateQuestionInstances()
        {
            foreach (Question question in this.Exam.Questions)
            {
                this.QuestionInstances.Add(question.CreateInstance());
            }
        }
    }
}