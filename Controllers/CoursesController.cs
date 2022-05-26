using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codetester.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
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
            // Get user id from JWT token
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _repository.GetUserById(userId);
            if (user.Role == UserRole.ADMIN)
            {
                var courses = _repository.GetAllCourses();
                return Ok(_mapper.Map<IEnumerable<CourseReadDto>>(courses));
            }
            else
            {
                var courses = _repository.GetAllCourses(user);
                return Ok(_mapper.Map<IEnumerable<CourseReadDto>>(courses));
            }

        }

        //GET api/courses/{coursecode}
        [HttpGet("{coursecode}", Name = "GetCourseByCourseCode")]
        public ActionResult<CourseReadDto> GetCourseByCourseCode(string coursecode)
        {
            // Get user id from JWT token
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _repository.GetUserById(userId);

            var course = _repository.GetCourseByCourseCode(coursecode);
            if (course == null)
            {
                return NotFound();
            }

            // if user is teacher, check access
            if (user.Role == UserRole.TEACHER)
            {
                if (course.Teachers.Contains(user) == false)
                {
                    return Forbid();
                }
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

            // Add current user to teachers
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _repository.GetUserById(userId);
    
            courseModel.Teachers.Add(user);

            _repository.CreateCourse(courseModel);
            _repository.SaveChanges();

            var courseReadDto = _mapper.Map<CourseReadDto>(courseModel);
            return CreatedAtRoute(nameof(GetCourseByCourseCode), new { CourseCode = courseReadDto.CourseCode }, courseReadDto);
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
            
            // Get user id from JWT token
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _repository.GetUserById(userId);

            // if user is teacher, check access
            if (user.Role == UserRole.TEACHER)
            {
                if (course.Teachers.Contains(user) == false)
                {
                    return Forbid();
                }
            }

            _repository.DeleteCourse(course);
            _repository.SaveChanges();
            return NoContent();
        }


    }
}