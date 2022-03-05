using System.Collections.Generic;
using AutoMapper;
using Codetester.Data;
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

        //GET api/courses/{id}
        [HttpGet("{id}", Name = "GetCourseById")]
        public ActionResult<CourseReadDto> GetCourseById(int id)
        {
            var course = _repository.GetCourseById(id);
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
            var courseModel = _mapper.Map<Course>(courseCreateDto);
            _repository.CreateCourse(courseModel);
            _repository.SaveChanges();

            var courseReadDto = _mapper.Map<CourseReadDto>(courseModel);
            return CreatedAtRoute(nameof(GetCourseById), new {Id = courseReadDto.Id}, courseReadDto);
        }

        //DELETE api/courses/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCourse(int id)
        {
            var course = _repository.GetCourseById(id);
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