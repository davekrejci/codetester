using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codetester.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    [Route("api/semesters")]
    [ApiController]
    public class SemestersController : ControllerBase
    {
        private readonly ICodetesterRepo _repository;
        private readonly IMapper _mapper;

        public SemestersController(ICodetesterRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/semesters
        [HttpGet]
        public ActionResult<IEnumerable<Semester>> GetAllSemesters()
        {
            var semesters = _repository.GetAllSemesters();
            return Ok(_mapper.Map<IEnumerable<SemesterReadDto>>(semesters));
        }

        //GET api/semesters/{id}
        [HttpGet("{id}", Name = "GetSemesterById")]
        public ActionResult<SemesterReadDto> GetSemesterById(int id)
        {
            var semester = _repository.GetSemesterById(id);
            if (semester == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<SemesterReadDto>(semester));
        }

        //POST api/semesters
        [HttpPost]
        public ActionResult<SemesterReadDto> CreateSemester(SemesterCreateDto semesterCreateDto)
        {
            var semesterModel = _mapper.Map<Semester>(semesterCreateDto);
            var course = _repository.GetCourseByCourseCode(semesterCreateDto.CourseCode);
            // Check semester is being created for existing course
            if (course == null)
            {
                return BadRequest("Request must contain a valid course ID");
            }
            // Check Year is valid year string
            var regex = new Regex(@"^\d{4}$");
            bool isValidYear = regex.IsMatch(semesterCreateDto.Year);
            if(!isValidYear)
            {
                return BadRequest("Year must be in format 'yyyy'");
            }
            // Check semester type is valid
            if (semesterCreateDto.SemesterType != SemesterType.SUMMER && semesterCreateDto.SemesterType != SemesterType.WINTER)
            {   
                return BadRequest("Semester type must be 'winter' or 'summer'");
            }
            // Check semester does not already exist for course
            if (course.Semesters.Any(s => s.Year == semesterCreateDto.Year && s.SemesterType == semesterCreateDto.SemesterType))
            {
                return BadRequest("Course " + course.CourseName + " already contains semester " + semesterCreateDto.Year + " - " + semesterCreateDto.SemesterType);
            }
            semesterModel.Course = course;
            _repository.CreateSemester(semesterModel);
            _repository.SaveChanges();

            var semesterReadDto = _mapper.Map<SemesterReadDto>(semesterModel);
            return CreatedAtRoute(nameof(GetSemesterById), new {Id = semesterReadDto.Id}, semesterReadDto);
        }

        //PUT api/semesters/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateSemester(int id, SemesterUpdateDto semesterUpdateDto)
        {
            var semesterModel = _repository.GetSemesterById(id);
            if (semesterModel == null)
            {
                return NotFound();
            }
            // map the enrolled students
            semesterModel.EnrolledStudents.Clear();
            foreach (var userId in semesterUpdateDto.UserIds)
            {
                var userModel = _repository.GetUserById(userId);
                if (userModel != null)
                {
                    semesterModel.EnrolledStudents.Add(userModel);
                } 
            }

            _repository.UpdateSemester(semesterModel);
            _repository.SaveChanges();
            return NoContent();
        }

        //DELETE api/courses/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteSemester(int id)
        {
            var semester = _repository.GetSemesterById(id);
            if (semester == null)
            {
                return NotFound();
            }
            _repository.DeleteSemester(semester);
            _repository.SaveChanges();
            return NoContent();
        }


    }
}