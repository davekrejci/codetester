using System.Collections.Generic;

namespace Codetester.Models
{
    public class CourseReadDto
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public ICollection<Semester> Semesters { get; set; }
    }
}