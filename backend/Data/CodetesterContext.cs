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
        public DbSet<MultiChoiceAnswer> MultiChoiceAnswers { get; set; }
        public DbSet<FillInCodeBlock> FillInCodeBlocks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamInstance> ExamInstances { get; set; }
        public DbSet<QuestionInstance> QuestionInstances { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Semester> Semesters { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set inherited question tables to TPT (table-per-type)
            // modelBuilder.Entity<Question>().ToTable("Questions");
            // modelBuilder.Entity<MultiChoiceQuestion>().ToTable("MultiChoiceQuestions");
            // modelBuilder.Entity<FillInCodeQuestion>().ToTable("FillInCodeQuestions");

            // Set inherited question tables to TPH (table-per-hierarchy) with discriminator column for each concrete type
            modelBuilder.Entity<Question>()
                .HasDiscriminator<string>("QuestionType")
                .HasValue<MultiChoiceQuestion>(QuestionType.MULTI_CHOICE)
                .HasValue<FillInCodeQuestion>(QuestionType.FILL_IN_CODE);

            modelBuilder.Entity<QuestionInstance>()
                .HasDiscriminator<string>("QuestionType")
                .HasValue<MultiChoiceQuestionInstance>(QuestionType.MULTI_CHOICE)
                .HasValue<FillInCodeQuestionInstance>(QuestionType.FILL_IN_CODE);
            
            modelBuilder.Entity<MultiChoiceQuestionInstance>()
                .HasMany(q => q.Answers)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FillInCodeQuestionInstance>()
                .HasMany(q => q.FillInCodeBlocks)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);


            // Set default user role
            modelBuilder.Entity<User>()
                .Property(user => user.Role).HasDefaultValue("Student");

            modelBuilder.Entity<QuestionInstance>()
                .Property(question => question.IsAnswered).HasDefaultValue(false);



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