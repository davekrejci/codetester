using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public abstract class Question
    {
        [Key]
        public int Id { get; set; }

        public ICollection<Tag> Tags { get; set; }
        public ICollection<Exam> Exams { get; set; }

        [Required]
        public string QuestionType { get; set; }

        [Required]
        public DateTime Created { get; set; }
        
        [DefaultValue(null)]
        public DateTime? Updated { get; set; }
        
        [DefaultValue(null)]
        public DateTime? Deleted { get; set; }


        public Question(){
            this.Tags = new List<Tag>();
        }     

        public abstract QuestionInstance CreateInstance();
    }

    public static class QuestionType
    {
        public const string FILL_IN_CODE = "fill-in-code";
        public const string MULTI_CHOICE = "multi-choice";

    }


}