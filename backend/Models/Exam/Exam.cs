using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Codetester.Models
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public Semester Semester { get; set; }
        
        [Required]
        public ICollection<Question> Questions { get; set; }
        
        public ICollection<Tag> Tags { get; set; }

        public ICollection<ExamInstance> ExamInstances { get; set; }
        
        public Exam(){
            this.Tags = new List<Tag>();
            this.Questions = new List<Question>();
        }

        
    }
}