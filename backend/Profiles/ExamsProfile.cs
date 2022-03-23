using AutoMapper;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Profiles
{
    public class ExamsProfile : Profile
    {
        public ExamsProfile()
        {
            CreateMap<Exam, ExamReadDto>();
            CreateMap<ExamCreateDto, Exam>()
                .ForMember(e => e.Tags, opt => opt.Ignore())
                .ForMember(e => e.Questions, opt => opt.Ignore());

            CreateMap<ExamUpdateDto, Exam>()
                .ForMember(e => e.Tags, opt => opt.Ignore())
                .ForMember(e => e.Questions, opt => opt.Ignore());
        }
    }
}