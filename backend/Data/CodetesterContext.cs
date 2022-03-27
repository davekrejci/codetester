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
        public DbSet<User> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Semester> Semesters { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set inherited question tables to TPT (table-per-type)
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<MultiChoiceQuestion>().ToTable("MultiChoiceQuestions");
            modelBuilder.Entity<FillInCodeQuestion>().ToTable("FillInCodeQuestions");

            // Seed admin user
            AuthUtil.CreatePasswordHash("admin", out byte[] passwordHash, out byte[] passwordSalt);
            modelBuilder.Entity<User>().HasData(
                    new User { 
                        Id = 1, 
                        Email = "admin@codetester.com",
                        Username = "admin", 
                        PasswordHash = passwordHash, 
                        PasswordSalt = passwordSalt,
                        Role = "Admin",
                        FirstName = "David",
                        LastName = "Krejci"
                    }
                );
        }

    }
}