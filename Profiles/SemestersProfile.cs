using AutoMapper;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Profiles
{
    public class SemestersProfile : Profile
    {
        public SemestersProfile()
        {
            CreateMap<Semester, SemesterReadDto>();
            CreateMap<Semester, SemesterReadNoCourseDto>();
            CreateMap<Semester, SemesterReadMinimalDto>();
            CreateMap<SemesterCreateDto, Semester>();
            CreateMap<SemesterUpdateDto, Semester>();
        }
    }
}