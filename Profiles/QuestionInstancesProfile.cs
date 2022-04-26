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

            CreateMap<QuestionInstance, QuestionInstanceResultDto>()
                .Include<MultiChoiceQuestionInstance, MultiChoiceQuestionInstanceResultDto>()
                .Include<FillInCodeQuestionInstance, FillInCodeQuestionInstanceResultDto>();


            CreateMap<MultiChoiceQuestionInstance, MultiChoiceQuestionInstanceAttemptReadDto>();
            CreateMap<MultiChoiceAnswerInstance, MultiChoiceAnswerAttemptReadDto>();

            CreateMap<MultiChoiceQuestionInstance, MultiChoiceQuestionInstanceResultDto>();
            CreateMap<MultiChoiceAnswerInstance, MultiChoiceAnswerResultDto>();

            CreateMap<FillInCodeQuestionInstance, FillInCodeQuestionInstanceAttemptReadDto>();
            CreateMap<FillInCodeBlockInstance, FillInCodeBlockInstanceAttemptDto>();

            CreateMap<FillInCodeQuestionInstance, FillInCodeQuestionInstanceResultDto>();
            CreateMap<FillInCodeBlockInstance, FillInCodeBlockInstanceResultDto>();
        }
    }
}