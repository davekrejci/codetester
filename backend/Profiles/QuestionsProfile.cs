using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Codetester.Dtos;
using Codetester.Models;
using Newtonsoft.Json.Linq;

namespace Codetester.Profiles
{
    public class QuestionsProfile : Profile
    {
        public QuestionsProfile()
        {

            

            // Source -> Target
            // Read DTOs
            CreateMap<Question, QuestionReadDto>()
                .Include<MultiChoiceQuestion, MultiChoiceQuestionReadDto>()
                .Include<FillInCodeQuestion, FillInCodeQuestionReadDto>();

            CreateMap<MultiChoiceQuestion, MultiChoiceQuestionReadDto>();
            CreateMap<FillInCodeQuestion, FillInCodeQuestionReadDto>();

            // Create DTOs
            CreateMap<QuestionCreateDto, Question>();
            CreateMap<QuestionCreateDto, MultiChoiceQuestion>();
            CreateMap<QuestionCreateDto, FillInCodeQuestion>();

            // Update DTOs
            CreateMap<QuestionUpdateDto, Question>();
            CreateMap<QuestionUpdateDto, MultiChoiceQuestion>();
            CreateMap<QuestionUpdateDto, FillInCodeQuestion>();

            CreateMap<Question, QuestionUpdateDto>();
            CreateMap<MultiChoiceQuestion, QuestionUpdateDto>();
            CreateMap<FillInCodeQuestion, QuestionUpdateDto>();


            CreateMap<FillInCodeBlock, FillInCodeBlockCreateDto>();
            CreateMap<FillInCodeBlockCreateDto, FillInCodeBlock>()
                .EqualityComparison((fdto, f) => fdto.Id == f.Id);

            CreateMap<FillInCodeBlockReadDto, FillInCodeBlock>().ReverseMap();

            CreateMap<MultiChoiceAnswer, MultiChoiceAnswerReadDto>();
            CreateMap<MultiChoiceAnswerCreateDto, MultiChoiceAnswer>()
                .EqualityComparison((adto, a) => adto.Id == a.Id);




        }
    }
}