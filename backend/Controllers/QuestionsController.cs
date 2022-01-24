using System.Collections.Generic;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
using Microsoft.AspNetCore.Mvc;

namespace Codetester.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ICodetesterRepo _repository;
        private readonly IMapper _mapper;

        public QuestionsController(ICodetesterRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/questions
        [HttpGet]
        public ActionResult <IEnumerable<Question>> GetAllQuestions()
        {
            var questions = _repository.GetAllQuestions();
            return Ok(_mapper.Map<IEnumerable<QuestionReadDto>>(questions));
        }

        //GET api/questions/{id}
        [HttpGet("{id}")]
        public ActionResult <QuestionReadDto> GetQuestionById(int id)
        {
            var question = _repository.GetQuestionById(id);
            if(question != null)
            {
                return Ok(_mapper.Map<QuestionReadDto>(question));
            }
            return NotFound();
        }
    }
}