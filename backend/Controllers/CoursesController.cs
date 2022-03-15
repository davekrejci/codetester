using System.Collections.Generic;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
using Microsoft.AspNetCore.Mvc;

namespace Codetester.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICodetesterRepo _repository;
        private readonly IMapper _mapper;

        public CoursesController(ICodetesterRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/courses
        [HttpGet]
        public ActionResult<IEnumerable<Question>> GetAllCourses()
        {
            var courses = _repository.GetAllCourses();
            return Ok(_mapper.Map<IEnumerable<CourseReadDto>>(courses));
        }

        //GET api/courses/{coursecode}
        [HttpGet("{coursecode}", Name = "GetCourseByCourseCode")]
        public ActionResult<CourseReadDto> GetCourseByCourseCode(string coursecode)
        {
            var course = _repository.GetCourseByCourseCode(coursecode);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CourseReadDto>(course));
        }

        //POST api/courses
        [HttpPost]
        public ActionResult<CourseReadDto> CreateCourse(CourseCreateDto courseCreateDto)
        {
            var course = _repository.GetCourseByCourseCode(courseCreateDto.CourseCode);
            // Check course does not already exist
            if (course != null)
            {
                return BadRequest("Course " + course.CourseName + " already exists");
            }
            var courseModel = _mapper.Map<Course>(courseCreateDto);
            _repository.CreateCourse(courseModel);
            _repository.SaveChanges();

            var courseReadDto = _mapper.Map<CourseReadDto>(courseModel);
            return CreatedAtRoute(nameof(GetCourseByCourseCode), new {CourseCode = courseReadDto.CourseCode}, courseReadDto);
        }

        //DELETE api/courses/{coursecode}
        [HttpDelete("{coursecode}")]
        public ActionResult DeleteCourse(string coursecode)
        {
            var course = _repository.GetCourseByCourseCode(coursecode);
            if (course == null)
            {
                return NotFound();
            }
            _repository.DeleteCourse(course);
            _repository.SaveChanges();
            return NoContent();
        }


    }
}