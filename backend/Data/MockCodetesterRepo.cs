using System.Collections.Generic;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Data
{
    public class MockCodetesterRepo : ICodetesterRepo
    {
        public void AddTagToQuestion(int tagId, int questionId)
        {
            throw new System.NotImplementedException();
        }

        public void CreateCourse(Course course)
        {
            throw new System.NotImplementedException();
        }

        public void CreateExam(Exam examModel)
        {
            throw new System.NotImplementedException();
        }

        public void CreateQuestion(Question question)
        {
            throw new System.NotImplementedException();
        }

        public void CreateSemester(Semester semester)
        {
            throw new System.NotImplementedException();
        }

        public void CreateTag(Tag tag)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCourse(Course course)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteExam(Exam examModel)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteQuestion(Question question)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteSemester(Semester semester)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Course> GetAllCourses()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Exam> GetAllExams()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            var questions = new List<Question>
            {
                new MultiChoiceQuestion{
                    Id=0, 
                    Tags=new List<Tag>{
                        new Tag{Id=0, TagText="testTag"},
                        new Tag{Id=1, TagText="testTag2"},
                    }, 
                    QuestionText="What is the airspeed velocity of an unladen swallow?",
                    Answers=new List<MultiChoiceAnswer>{
                        new MultiChoiceAnswer{AnswerText="test", IsCorrect=false},
                        new MultiChoiceAnswer{AnswerText="test2", IsCorrect=true},
                        new MultiChoiceAnswer{AnswerText="test3", IsCorrect=true},
                    }
                },
                new FillInCodeQuestion{
                    Id=2,
                    CodeDescription="A HelloWorld class",
                    Code="public class HelloWorld(){}",
                    FillCount=1,
                    FillInCodeBlocks=new List<FillInCodeBlock>{
                        new FillInCodeBlock{Content="class", StartPosition=8, EndPosition=12}
                    }
                }
            };
            return questions;
        }

        public IEnumerable<Semester> GetAllSemesters()
        {
            throw new System.NotImplementedException();
        }

        public Course GetCourseByCourseCode(string coursecode)
        {
            throw new System.NotImplementedException();
        }

        public Exam GetExamById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Question GetQuestionById(int id)
        {
            return new MultiChoiceQuestion{
                    Id=0, 
                    Tags=new List<Tag>{
                        new Tag{TagText="test"}
                    }, 
                    QuestionText="What is the airspeed velocity of an unladen swallow?",
                    Answers=new List<MultiChoiceAnswer>{
                        new MultiChoiceAnswer{AnswerText="test", IsCorrect=false},
                        new MultiChoiceAnswer{AnswerText="test2", IsCorrect=true},
                        new MultiChoiceAnswer{AnswerText="test3", IsCorrect=true},
                    }
                };
        }

        public Semester GetSemesterById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateExam(Exam examModel)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateQuestion(Question question)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateQuestion(Question question, QuestionUpdateDto questionUpdateDTO)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateSemester(Semester semester)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<Tag> ICodetesterRepo.GetAllTags()
        {
            throw new System.NotImplementedException();
        }

        Tag ICodetesterRepo.GetTagById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}