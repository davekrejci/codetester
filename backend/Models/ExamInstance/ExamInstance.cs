using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Codetester.Dtos;
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

        public bool IsCompleted { get; set; }

        public int MaxScore { get; set; }

        public int Score { get; set; }
        
        public ExamInstance()
        {
            // default
        }

        public ExamInstance(Exam exam, User user)
        {
            this.Exam = exam;
            this.User = user;
            this.IsCompleted = false;
            this.QuestionInstances = new List<QuestionInstance>();
            GenerateQuestionInstances();
            this.MaxScore = this.QuestionInstances.Sum(qi => qi.MaxScore);
            this.Score = 0;
        }

        private void GenerateQuestionInstances()
        {
            foreach (Question question in this.Exam.Questions)
            {
                this.QuestionInstances.Add(question.CreateInstance());
            }
        }

        public void EvaluateExamAttempt(ExamInstanceAttemptTurnInDto attempt)
        {
            int score = 0;
            foreach (var questionInstance in this.QuestionInstances)
            {
                QuestionInstanceAttemptTurnInDto questionInstanceAttempt = attempt.QuestionInstances.FirstOrDefault(qi => qi.Id == questionInstance.Id);
                questionInstance.EvaluateQuestionAttempt(questionInstanceAttempt);
                score += questionInstance.Score;
            }
            
            this.Score = score;
            this.IsCompleted = true;
        }
    }
}