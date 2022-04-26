using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Profiles
{
    public class TagsProfile : Profile
    {
        public TagsProfile()
        {
            CreateMap<Tag, TagReadDto>();
            CreateMap<TagCreateDto, Tag>().ReverseMap()
                .EqualityComparison((tagDto, tag) => tagDto.Id == tag.Id);

        }
    }
}