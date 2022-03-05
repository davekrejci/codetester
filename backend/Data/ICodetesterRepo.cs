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
        Course GetCourseById(int id);
        void CreateCourse(Course courseModel);
        void DeleteCourse(Course course);
    }
}