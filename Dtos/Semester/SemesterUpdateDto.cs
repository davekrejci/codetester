using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Codetester.Models;

namespace Codetester.Dtos
{
    public class SemesterUpdateDto
    {
        public int[] UserIds { get; set; }
    }
}