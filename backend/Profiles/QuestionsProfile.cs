using AutoMapper;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Profiles
{
    public class QuestionsProfile : Profile
    {
        public QuestionsProfile()
        {
            CreateMap<Question, QuestionReadDto>();
            CreateMap<QuestionCreateDto, Question>();
        }
    }
}