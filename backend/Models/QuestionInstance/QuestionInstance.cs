using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

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

        public QuestionInstance(){
            
        }
    }

}