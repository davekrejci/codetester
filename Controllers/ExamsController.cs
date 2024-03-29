using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Codetester.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    [Route("api/exams")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly ICodetesterRepo _repository;
        private readonly IMapper _mapper;

        public ExamsController(ICodetesterRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/exams
        [HttpGet]
        public ActionResult<IEnumerable<Exam>> GetAllExams()
        {
            // Get user id from JWT token
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _repository.GetUserById(userId);
            if (user.Role == UserRole.ADMIN)
            {
                var exams = _repository.GetAllExams();
                return Ok(_mapper.Map<IEnumerable<ExamReadDto>>(exams));
            }
            else {
                var exams = _repository.GetAllExams(user);
                return Ok(_mapper.Map<IEnumerable<ExamReadDto>>(exams));
            }
        }

        //GET api/exams/{id}
        [HttpGet("{id}", Name = "GetExamById")]
        public ActionResult<ExamReadDto> GetExamById(int id)
        {
            // Get user id from JWT token
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _repository.GetUserById(userId);

            var exam = _repository.GetExamById(id);
            if (exam == null)
            {
                return NotFound();
            }

            // if user is not admin and is not a teacher in the exams course, forbid access
            if(user.Role != UserRole.ADMIN && exam.Semester.Course.Teachers.Contains(user) == false) {
                return Forbid();
            }

            return Ok(_mapper.Map<ExamReadDto>(exam));
        }

        //POST api/exams
        [HttpPost]
        public ActionResult<ExamReadDto> CreateExam(ExamCreateDto examCreateDto)
        {
            System.Console.WriteLine(JsonConvert.SerializeObject(examCreateDto));
            var examModel = _mapper.Map<Exam>(examCreateDto);

            // check the date and set status
            if (examCreateDto.StartDate.ToUniversalTime() < DateTime.UtcNow)
            {
                return BadRequest("Start date must be in future");
            }
            if (examCreateDto.EndDate < examCreateDto.StartDate)
            {
                return BadRequest("End date must be later than start date");
            }
            examModel.StartDate = examModel.StartDate.ToUniversalTime();
            examModel.EndDate = examModel.EndDate.ToUniversalTime();

            // map the semester
            var semesterModel = _repository.GetSemesterById(examCreateDto.SemesterId);
            if (semesterModel == null)
            {
                return BadRequest("No semester with id " + examCreateDto.SemesterId + " found");
            }
            else
            {
                examModel.Semester = semesterModel;
            }

            // map the tags
            foreach (TagCreateDto tagDto in examCreateDto.Tags)
            {
                var tagModel = _repository.GetTagById(tagDto.Id);
                if (tagModel != null)
                {
                    examModel.Tags.Add(tagModel);
                }
                else
                {
                    var newTag = _mapper.Map<TagCreateDto, Tag>(tagDto);
                    examModel.Tags.Add(newTag);
                }
            }

            // map the questions
            foreach (int id in examCreateDto.QuestionIds)
            {
                var questionModel = _repository.GetQuestionById(id);
                if (questionModel != null)
                {
                    examModel.Questions.Add(questionModel);
                }
            }

            _repository.CreateExam(examModel);


            // Create specific exam instances for each user
            foreach (User user in examModel.Semester.EnrolledStudents)
            {
                ExamInstance examInstance = new ExamInstance(examModel, user);
                _repository.CreateExamInstance(examInstance);
            }
            
            _repository.SaveChanges();

            var examReadDto = _mapper.Map<ExamReadDto>(examModel);
            return CreatedAtRoute(nameof(GetExamById), new { Id = examReadDto.Id }, examReadDto);
        }

        //PUT api/exams/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateExam(int id, ExamUpdateDto examUpdateDto)
        {
            var examModel = _repository.GetExamById(id);
            if (examModel == null)
            {
                return NotFound();
            }
            _mapper.Map(examUpdateDto, examModel);
            // check the date and set status
            if (examUpdateDto.StartDate.ToUniversalTime() < DateTime.UtcNow)
            {
                return BadRequest("Start date must be in future");
            }
            if (examUpdateDto.EndDate < examUpdateDto.StartDate)
            {
                return BadRequest("End date must be later than start date");
            }
            examModel.StartDate = examModel.StartDate.ToUniversalTime();
            examModel.EndDate = examModel.EndDate.ToUniversalTime();

            // map the semester
            var semesterModel = _repository.GetSemesterById(examUpdateDto.SemesterId);
            if (semesterModel == null)
            {
                return BadRequest("No semester with id " + examUpdateDto.SemesterId + " found");
            }
            else
            {
                examModel.Semester = semesterModel;
            }

            // map the tags
            examModel.Tags.Clear();
            foreach (TagCreateDto tagDto in examUpdateDto.Tags)
            {
                var tagModel = _repository.GetTagById(tagDto.Id);
                if (tagModel != null)
                {
                    examModel.Tags.Add(tagModel);
                }
                else
                {
                    var newTag = _mapper.Map<TagCreateDto, Tag>(tagDto);
                    examModel.Tags.Add(newTag);
                }
            }

            // map the questions
            examModel.Questions.Clear();
            foreach (int questionId in examUpdateDto.QuestionIds)
            {
                var questionModel = _repository.GetQuestionById(questionId);
                if (questionModel != null)
                {
                    examModel.Questions.Add(questionModel);
                }
            }
            
            _repository.UpdateExam(examModel);
            _repository.SaveChanges();

            return NoContent();
        }

        //DELETE api/exams/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteExam(int id)
        {
            // Get user id from JWT token
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _repository.GetUserById(userId);

            var exam = _repository.GetExamById(id);
            if (exam == null)
            {
                return NotFound();
            }

            // if user is not admin and is not a teacher in the exams course, forbid access
            var courseCode = exam.Semester.Course.CourseCode;
            var course = _repository.GetCourseByCourseCode(courseCode);
            if(user.Role != UserRole.ADMIN && course.Teachers.Contains(user) == false) {
                return Forbid();
            }
            
            _repository.DeleteExam(exam);
            _repository.SaveChanges();
            return NoContent();
        }
    }
}