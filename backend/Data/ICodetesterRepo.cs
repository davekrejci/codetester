using System.Collections.Generic;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Data
{
    public interface ICodetesterRepo
    {
        bool SaveChanges();

        // QUESTIONS
        IEnumerable<Question> GetAllQuestions();
        Question GetQuestionById(int id);
        void CreateQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteQuestion(Question question);

        // TAGS
        IEnumerable<Tag> GetAllTags();
        Tag GetTagById(int id);
        void CreateTag(Tag tag);
        void AddTagToQuestion(int tagId, int questionId);

        // COURSES
        IEnumerable<Course> GetAllCourses();
        Course GetCourseByCourseCode(string coursecode);
        void CreateCourse(Course course);
        void DeleteCourse(Course course);
        
        // SEMESTERS
        IEnumerable<Semester> GetAllSemesters();
        Semester GetSemesterById(int id);
        void CreateSemester(Semester semester);
        void UpdateSemester(Semester semester);
        void DeleteSemester(Semester semester);

        // EXAMS
        IEnumerable<Exam> GetAllExams();
        Exam GetExamById(int id);
        void CreateExam(Exam examModel);
        void UpdateExam(Exam examModel);
        void DeleteExam(Exam examModel);

        // USERS
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByUsername(string username);
        void CreateUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);
    }
}