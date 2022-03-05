using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Course, CourseReadDto>();
            CreateMap<CourseCreateDto, Course>();
        }
    }
}