using System;
using System.Collections.Generic;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
using Microsoft.AspNetCore.Mvc;

namespace Codetester.Controllers
{
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
            var exams = _repository.GetAllExams();
            return Ok(_mapper.Map<IEnumerable<ExamReadDto>>(exams));
        }

        //GET api/exams/{id}
        [HttpGet("{id}", Name = "GetExamById")]
        public ActionResult<ExamReadDto> GetExamById(int id)
        {
            var exam = _repository.GetExamById(id);
            if (exam == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ExamReadDto>(exam));
        }

        //POST api/exams
        [HttpPost]
        public ActionResult<ExamReadDto> CreateExam(ExamCreateDto examCreateDto)
        {
            var examModel = _mapper.Map<Exam>(examCreateDto);

            // check the date and set status
            var now = new DateTime();
            if (examCreateDto.StartDate < now) {
                return BadRequest("Start date must be in future");
            }
            if (examCreateDto.EndDate < examCreateDto.StartDate) {
                return BadRequest("End date must be later than start date");
            }
            examModel.Status = "planned";

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
                } else {
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
            _repository.SaveChanges();

            var examReadDto = _mapper.Map<ExamReadDto>(examModel);
            return CreatedAtRoute(nameof(GetExamById), new {Id = examReadDto.Id}, examReadDto);
        }

        //DELETE api/exams/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteExam(int id)
        {
            var exam = _repository.GetExamById(id);
            if (exam == null)
            {
                return NotFound();
            }
            _repository.DeleteExam(exam);
            _repository.SaveChanges();
            return NoContent();
        }


    }
}