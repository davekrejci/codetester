using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Codetester.Dtos;
using Newtonsoft.Json;

namespace Codetester.Models
{
    public class ExamInstanceResultListDto
    {
        [JsonConverter(typeof(HashIdJsonConverter))]
        public int Id { get; set; }
        public UserReadDto User { get; set; }
        public bool IsCompleted { get; set; }
        public int MaxScore { get; set; }
        public int Score { get; set; }
    }
}