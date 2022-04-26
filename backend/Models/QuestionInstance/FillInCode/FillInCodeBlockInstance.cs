using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codetester.Models
{
    public class FillInCodeBlockInstance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string WidgetId { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        [Required]
        public int StartPosition { get; set; }
        
        [Required]
        public int EndPosition { get; set; }
        
        public string Answer { get; set; }

        [NotMapped]
        public bool IsCorrect
        { 
            get
            {
                if (Answer == Content)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set { }
        }


        public FillInCodeBlockInstance()
        {
            //nothing
        }
        public FillInCodeBlockInstance(FillInCodeBlock block)
        {
            this.Content = block.Content;
            this.StartPosition = block.StartPosition;
            this.EndPosition = block.EndPosition;
        }

    }
}