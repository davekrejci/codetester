using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CourseCode { get; set; }
        [Required]
        public string CourseName { get; set; }
        public ICollection<Semester> Semesters { get; set; }
        public ICollection<User> Teachers { get; set; }

        public Course(){
            this.Teachers = new List<User>();
        }   
    }
}