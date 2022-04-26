using System.Collections.Generic;

namespace Codetester.Dtos
{
    public class CourseReadDto
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public ICollection<SemesterReadNoCourseDto> Semesters { get; set; }
    }
}