using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class TagCreateDto
    {
        public int Id { get; set; }
        [Required]
        public string TagText { get; set; }

    }
}