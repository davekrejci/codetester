using System.Collections.Generic;
using Codetester.Models;

namespace Codetester.Dtos
{
    public class QuestionReadDto
    {
        public int Id { get; set; }

        public ICollection<TagReadDto> Tags { get; set; }

        public string QuestionType { get; set; }
        
    }
}