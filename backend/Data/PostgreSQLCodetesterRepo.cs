using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Codetester.Models;
using Microsoft.EntityFrameworkCore;

namespace Codetester.Data
{
    public class PostgreSQLCodetesterRepo : ICodetesterRepo
    {
        private readonly CodetesterContext _context;

        public PostgreSQLCodetesterRepo(CodetesterContext context)
        {
            _context = context;
        }
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        // QUESTIONS
        public void CreateQuestion(Question question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _context.Questions.Add(question);
        }
        public void DeleteQuestion(Question question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            _context.Questions.Remove(question);
        }
        public IEnumerable<Question> GetAllQuestions()
        {
            return _context.Questions
                                .Include("Answers")
                                .Include("FillInCodeBlocks")
                                .Include("Tags")
                                .ToList();
        }
        public Question GetQuestionById(int id)
        {
            return _context.Questions
                            .Include("Answers")
                            .Include("FillInCodeBlocks")
                            .Include("Tags")
                            .FirstOrDefault(q => q.Id == id);

        }
        public void UpdateQuestion(Question question)
        {
            // Nothing, updated automatically in controller thanks to mapping from DTO to repo model
            // Keep here for sake of Interface implementation and/or possible future implementation changes
        }

        // TAGS
        public IEnumerable<Tag> GetAllTags()
        {
            return _context.Tags.ToList();
        }
        public Tag GetTagById(int id)
        {
            return _context.Tags.FirstOrDefault(t => t.Id == id);
        }
        public void AddTagToQuestion(int tagId, int questionId)
        {
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == tagId);
            Question question = _context.Questions.FirstOrDefault(q => q.Id == questionId);

            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            question.Tags.Add(tag);
        }
        public void CreateTag(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            _context.Tags.Add(tag);
        }

        // COURSES
        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses
                            .Include("Semesters")
                            .ToList();
        }
        public Course GetCourseByCourseCode(string coursecode)
        {
            return _context.Courses
                            .Include("Semesters")
                            .FirstOrDefault(c => c.CourseCode == coursecode);
        }
        public void CreateCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            _context.Courses.Add(course);
        }
        public void DeleteCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            _context.Courses.Remove(course);
        }

        // SEMESTERS
        public IEnumerable<Semester> GetAllSemesters()
        {
            return _context.Semesters
                            .Include("Course")
                            .Include("Exams")
                            .Include("EnrolledStudents")
                            .ToList();
        }
        public Semester GetSemesterById(int id)
        {
            return _context.Semesters
                            .Include("Exams.Tags")
                            .Include("EnrolledStudents")
                            .Include("Course")
                            .FirstOrDefault(s => s.Id == id);
        }
        public void CreateSemester(Semester semester)
        {
            if (semester == null)
            {
                throw new ArgumentNullException(nameof(semester));
            }

            _context.Semesters.Add(semester);
        }
        public void UpdateSemester(Semester semester)
        {
            // Nothing, updated automatically in controller thanks to mapping from DTO to repo model
            // Keep here for sake of Interface implementation and/or possible future implementation changes
        }
        public void DeleteSemester(Semester semester)
        {
            if (semester == null)
            {
                throw new ArgumentNullException(nameof(semester));
            }
            _context.Semesters.Remove(semester);
        }

        // EXAMS
        public IEnumerable<Exam> GetAllExams()
        {
            return _context.Exams
                            .Include("Questions.Tags")
                            .Include("Tags")
                            .Include("Semester.Course")
                            .ToList();
        }
        public Exam GetExamById(int id)
        {
            return _context.Exams
                            .Include("Questions.Tags")
                            .Include("Tags")
                            .Include("Semester.Course")
                            .FirstOrDefault(e => e.Id == id);
        }
        public void CreateExam(Exam exam)
        {
            if (exam == null)
            {
                throw new ArgumentNullException(nameof(exam));
            }

            _context.Exams.Add(exam);
        }
        public void UpdateExam(Exam exam)
        {
            // Nothing, updated automatically in controller thanks to mapping from DTO to repo model
            // Keep here for sake of Interface implementation and/or possible future implementation changes
        }
        public void DeleteExam(Exam exam)
        {
            if (exam == null)
            {
                throw new ArgumentNullException(nameof(exam));
            }
            _context.Exams.Remove(exam);
        }

        // EXAM INSTANCE
        public void CreateExamInstance(ExamInstance examInstance)
        {
            if (examInstance == null)
            {
                throw new ArgumentNullException(nameof(examInstance));
            }

            _context.ExamInstances.Add(examInstance);
        }

        public IEnumerable<ExamInstance> GetAllExamInstances()
        {
            return _context.ExamInstances
                            .Include("Exam")
                            .Include("QuestionInstances")
                            .ToList();
        }

        public IEnumerable<ExamInstance> GetUsersExamInstances(User user)
        {
            return _context.ExamInstances
                            .Where(e => e.User == user)
                            .Include("Exam.Semester.Course")
                            .ToList();
        }

        public ExamInstance GetExamInstanceById(int id)
        {
            return _context.ExamInstances
                            .Include("Exam.Semester.Course")
                            .Include("QuestionInstances.Answers")
                            .FirstOrDefault(e => e.Id == id);
        }
    
    
        // USERS
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users
                            .ToList();
        }
        public User GetUserById(int id)
        {
            return _context.Users
                            .FirstOrDefault(u => u.Id == id);
        }
        public User GetUserByUsername(string username)
        {
            return _context.Users
                            .FirstOrDefault(u => u.Username == username);
        }
        public void CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Add(user);
        }
        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Users.Remove(user);
        }

        public void UpdateUser(User user)
        {
           // Nothing, updated automatically in controller thanks to mapping from DTO to repo model
           // Keep here for sake of Interface implementation and/or possible future implementation changes
        }

    }
}