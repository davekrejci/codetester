using System.Collections.Generic;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
using Microsoft.AspNetCore.Mvc;

namespace Codetester.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ICodetesterRepo _repository;
        private readonly IMapper _mapper;

        public TagsController(ICodetesterRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/tags
        [HttpGet]
        public ActionResult<IEnumerable<Question>> GetAllTags()
        {
            var tags = _repository.GetAllTags();
            return Ok(_mapper.Map<IEnumerable<TagReadDto>>(tags));
        }

        //GET api/tags/{id}
        [HttpGet("{id}", Name = "GetTagById")]
        public ActionResult<QuestionReadDto> GetTagById(int id)
        {
            var tag = _repository.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TagReadDto>(tag));
        }

        //POST api/tags
        [HttpPost]
        public ActionResult<QuestionReadDto> CreateTag(TagCreateDto tagCreateDto)
        {
            // todo
            throw new System.NotImplementedException();
        }

        //DELETE api/tags/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteTag(int id)
        {
            // todo
            throw new System.NotImplementedException();
        }


    }
}