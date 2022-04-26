using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class Semester
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Year { get; set; }

        [Required]
        public string SemesterType { get; set; }

        [Required]
        public Course Course { get; set; }

        public ICollection<User> EnrolledStudents { get; set; }
        public ICollection<Exam> Exams { get; set; }

    }

    public static class SemesterType
    {
        public const string WINTER = "winter";
        public const string SUMMER = "summer";

    }
}