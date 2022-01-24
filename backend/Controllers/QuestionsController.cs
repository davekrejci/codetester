using System.Collections.Generic;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
using Microsoft.AspNetCore.JsonPatch;
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
        [HttpGet("{id}", Name="GetQuestionById")]
        public ActionResult <QuestionReadDto> GetQuestionById(int id)
        {
            var question = _repository.GetQuestionById(id);
            if(question == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<QuestionReadDto>(question));
        }

        //POST api/questions
        [HttpPost]
        public ActionResult <QuestionReadDto> CreateQuestion(QuestionCreateDto questionCreateDto)
        {
            var questionModel = _mapper.Map<Question>(questionCreateDto);
            _repository.CreateQuestion(questionModel);
            _repository.SaveChanges();

            var questionReadDto = _mapper.Map<QuestionReadDto>(questionModel);
            return CreatedAtRoute(nameof(GetQuestionById), new {Id = questionReadDto.Id}, questionReadDto);
        }

        //PUT api/questions/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateQuestion(int id, QuestionUpdateDto questionUpdateDto)
        {
            var question = _repository.GetQuestionById(id);
            if(question == null)
            {
                return NotFound();
            }
            _mapper.Map(questionUpdateDto, question);
            _repository.UpdateQuestion(question);
            _repository.SaveChanges();
            return NoContent();
        }

        //PATCH api/questions/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialQuestionUpdate(int id, JsonPatchDocument<QuestionUpdateDto> patchDoc)
        {
            var question = _repository.GetQuestionById(id);
            if(question == null)
            {
                return NotFound();
            }

            var questionToPatch = _mapper.Map<QuestionUpdateDto>(question);
            patchDoc.ApplyTo(questionToPatch, ModelState);
            if(!TryValidateModel(questionToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(questionToPatch, question);
            _repository.UpdateQuestion(question);
            _repository.SaveChanges();
            return NoContent();
        }

    }
}