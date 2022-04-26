using AutoMapper;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Profiles
{
    public class ExamInstancesProfile : Profile
    {
        public ExamInstancesProfile()
        {
            CreateMap<ExamInstance, ExamInstanceListReadDto>();
            CreateMap<ExamInstance, ExamInstanceAttemptReadDto>();
            CreateMap<ExamInstance, ExamInstanceResultDto>();
            CreateMap<ExamInstance, ExamInstanceResultListDto>();
        }
    }
}