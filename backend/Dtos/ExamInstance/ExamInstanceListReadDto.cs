using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codetester.Dtos;
using Newtonsoft.Json;

namespace Codetester.Dtos
{
    public class ExamInstanceListReadDto
    {
        public int Id { get; set; }

        public ExamReadMinimalDto Exam { get; set; }
        
    }
}