using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Codetester.Dtos;

namespace Codetester.Models
{
    public abstract class QuestionInstance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string QuestionType { get; set; }

        [Required]
        public ExamInstance ExamInstance { get; set; }

        public bool IsAnswered { get; set; }

        public string State { get; set; }
        
        public int MaxScore { get; set; }
        public int Score { get; internal set; }

        public QuestionInstance(){
            
        }

        public abstract void EvaluateQuestionAttempt(QuestionInstanceAttemptTurnInDto questionInstanceAttempt);

    }

    public static class QuestionInstanceState
    {
        public const string CORRECT = "correct";
        public const string INCORRECT = "incorrect";
        public const string PARTIALLYCORRECT = "partiallycorrect";
    }

}