using Codetester.Models;
using Microsoft.EntityFrameworkCore;

namespace Codetester.Data
{
    public class CodetesterContext : DbContext
    {
        public CodetesterContext(DbContextOptions<CodetesterContext> opt) : base(opt)
        {

        }

        public DbSet<Question> Questions { get; set; }
        
    }
}