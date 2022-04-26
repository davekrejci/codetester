using System;
using System.Collections.Generic;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Codetester.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
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
        public ActionResult<IEnumerable<Question>> GetAllQuestions()
        {
            var questions = _repository.GetAllQuestions();
            return Ok(_mapper.Map<IEnumerable<QuestionReadDto>>(questions));
        }

        //GET api/questions/{id}
        [HttpGet("{id}", Name = "GetQuestionById")]
        public ActionResult<QuestionReadDto> GetQuestionById(int id)
        {
            var question = _repository.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<QuestionReadDto>(question));
        }

        //POST api/questions
        [HttpPost]
        public ActionResult<QuestionReadDto> CreateQuestion(QuestionCreateDto questionCreateDto)
        {
            Question questionModel;
            if (questionCreateDto.QuestionType == QuestionType.MULTI_CHOICE)
            {
                questionModel = _mapper.Map<MultiChoiceQuestion>(questionCreateDto);
            }
            else if (questionCreateDto.QuestionType == QuestionType.FILL_IN_CODE)
            {
                questionModel = _mapper.Map<FillInCodeQuestion>(questionCreateDto);
            }
            else 
            {
                // return error message that question type not valid
                return BadRequest("Question type " + questionCreateDto.QuestionType + " is not a valid type");
            }
        
            // map the tags
            foreach (TagCreateDto tagDto in questionCreateDto.Tags)
            {
                var tagModel = _repository.GetTagById(tagDto.Id);
                if (tagModel != null)
                {
                    questionModel.Tags.Add(tagModel);
                } else {
                    var newTag = _mapper.Map<TagCreateDto, Tag>(tagDto);
                    questionModel.Tags.Add(newTag);
                }
            }

            //set created time
            questionModel.Created = DateTime.UtcNow;

            _repository.CreateQuestion(questionModel);
            _repository.SaveChanges();
            

            var questionReadDto = _mapper.Map<QuestionReadDto>(questionModel);
            return CreatedAtRoute(nameof(GetQuestionById), new {Id = questionReadDto.Id}, questionReadDto);
        }

        //PUT api/questions/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateQuestion(int id, QuestionUpdateDto questionUpdateDto)
        {
            var questionModel = _repository.GetQuestionById(id);
            if (questionModel == null)
            {
                return NotFound();
            }
            if (questionUpdateDto.QuestionType != questionModel.QuestionType)
            {
                return BadRequest("Not possible to change the question type for existing question.");
            }

            // set updated time
            questionModel.Updated = DateTime.UtcNow;

            _mapper.Map(questionUpdateDto, questionModel);
            _repository.UpdateQuestion(questionModel);
            
            _repository.SaveChanges();
            return NoContent();
        }

        //PATCH api/questions/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialQuestionUpdate(int id, JsonPatchDocument<QuestionUpdateDto> patchDoc)
        {
            var question = _repository.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }
            var questionToPatch = _mapper.Map<QuestionUpdateDto>(question);
            patchDoc.ApplyTo(questionToPatch, ModelState);
            if (!TryValidateModel(questionToPatch))
            {
                return ValidationProblem(ModelState);
            }
            if (questionToPatch.QuestionType != question.QuestionType)
            {
                return BadRequest("Not possible to change the question type for existing question.");
            }

            _mapper.Map(questionToPatch, question);
            _repository.UpdateQuestion(question);
            _repository.SaveChanges();
            return NoContent();
        }

        //DELETE api/questions/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteQuestion(int id)
        {
            var question = _repository.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }

            // set deleted time
            question.Deleted = DateTime.UtcNow;

            _repository.UpdateQuestion(question);
            _repository.SaveChanges();
            return NoContent();
        }


    }
}