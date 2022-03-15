using AutoMapper;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Course, CourseReadDto>();
            CreateMap<Course, CourseReadMinimalDto>();
            CreateMap<CourseCreateDto, Course>();
        }
    }
}