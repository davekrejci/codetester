using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class Question
    {


        [Key]
        public int Id { get; set; }

        public ICollection<Tag> Tags { get; set; }

        [Required]
        public string QuestionType { get; set; }


        public Question(){
            this.Tags = new List<Tag>();
        }

        
    }

    public static class QuestionType
    {
        public const string FILL_IN_CODE = "fill-in-code";
        public const string MULTI_CHOICE = "multi-choice";

    }


}