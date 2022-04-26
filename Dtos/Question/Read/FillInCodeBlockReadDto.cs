using System.ComponentModel.DataAnnotations;

namespace Codetester.Dtos
{
    public class FillInCodeBlockReadDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }

    }
}