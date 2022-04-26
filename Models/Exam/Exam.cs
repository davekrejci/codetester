using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codetester.Models
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [NotMapped]
        public string Status
        {
            get
            {
                if (this.StartDate > DateTime.UtcNow)
                {
                    return ExamState.PLANNED;
                }
                if (DateTime.UtcNow < this.EndDate)
                {
                    return ExamState.OPEN;
                }
                return ExamState.CLOSED;
            }
            set { }
        }

        [Required]
        public Semester Semester { get; set; }

        [Required]
        public ICollection<Question> Questions { get; set; }

        public string Description { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<ExamInstance> ExamInstances { get; set; }

        public Exam()
        {
            this.Tags = new List<Tag>();
            this.Questions = new List<Question>();
        }

    }
    public static class ExamState
    {
        public const string PLANNED = "planned";
        public const string OPEN = "open";
        public const string CLOSED = "closed";
    }
}