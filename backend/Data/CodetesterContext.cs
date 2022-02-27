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
        public DbSet<MultiChoiceQuestion> MultiChoiceQuestions { get; set; }
        public DbSet<MultiChoiceAnswer> MultiChoiceAnswers { get; set; }
        public DbSet<FillInCodeQuestion> FillInCodeQuestions { get; set; }
        public DbSet<FillInCodeBlock> FillInCodeBlocks { get; set; }
        public DbSet<Tag> Tags { get; set; }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<MultiChoiceQuestion>().ToTable("MultiChoiceQuestions");
            modelBuilder.Entity<FillInCodeQuestion>().ToTable("FillInCodeQuestions");
        }

    }
}