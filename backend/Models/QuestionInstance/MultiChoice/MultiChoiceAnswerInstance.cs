using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class MultiChoiceAnswerInstance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AnswerText { get; set; }
        
        [Required]
        public bool IsCorrect { get; set; }

        [Required]
        public bool IsSelected { get; set; }


        public MultiChoiceAnswerInstance()
        {
            //nothing
        }
        public MultiChoiceAnswerInstance(MultiChoiceAnswer answer)
        {
            this.AnswerText = answer.AnswerText;
            this.IsCorrect = answer.IsCorrect;
            this.IsSelected = false;
        }

    }
}