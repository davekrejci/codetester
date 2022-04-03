using AutoMapper;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Profiles
{
    public class QuestionInstancesProfile : Profile
    {
        public QuestionInstancesProfile()
        {
            CreateMap<QuestionInstance, QuestionInstanceAttemptReadDto>()
                .Include<MultiChoiceQuestionInstance, MultiChoiceQuestionInstanceAttemptReadDto>()
                .Include<FillInCodeQuestionInstance, FillInCodeQuestionInstanceAttemptReadDto>();


            CreateMap<MultiChoiceQuestionInstance, MultiChoiceQuestionInstanceAttemptReadDto>();
            CreateMap<FillInCodeQuestionInstance, FillInCodeQuestionInstanceAttemptReadDto>();

        }
    }
}