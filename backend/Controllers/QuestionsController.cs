using System.Collections.Generic;
using Codetester.Data;
using Codetester.Models;
using Microsoft.AspNetCore.Mvc;

namespace Codetester.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ICodetesterRepo _repository;
        public QuestionsController(ICodetesterRepo repository)
        {
            _repository = repository;
        }

        //GET api/questions
        [HttpGet]
        public ActionResult <IEnumerable<Question>> GetAllQuestions()
        {
            var questions = _repository.GetAllQuestions();
            return Ok(questions);
        }

        //GET api/questions/{id}
        [HttpGet("{id}")]
        public ActionResult <Question> GetQuestionById(int id)
        {
            var question = _repository.GetQuestionById(id);
            return Ok(question);
        }
    }
}