using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TagText { get; set; }

        public ICollection<Question> Questions { get; set; }

    }
}