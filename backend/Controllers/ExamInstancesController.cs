using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Codetester.Controllers
{
    [Authorize]
    [Route("api/examinstances")]
    [ApiController]
    public class ExamInstancesController : ControllerBase
    {
        private readonly ICodetesterRepo _repository;
        private readonly IMapper _mapper;

        public ExamInstancesController(ICodetesterRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/examinstances/all
        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet("all")]
        public ActionResult<IEnumerable<ExamInstanceListReadDto>> GetAllExamInstances()
        {
            var examInstances = _repository.GetAllExamInstances();
            
            //return Ok(_mapper.Map<IEnumerable<ExamInstanceReadDto>>(exams));
            return Ok(examInstances);
        }

        //GET api/examinstances
        [HttpGet]
        public ActionResult<IEnumerable<ExamInstanceListReadDto>> GetUsersExamInstances()
        {
            // Get user id from JWT token
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            // check that user exists
            var user = _repository.GetUserById(userId);
            if (user == null)
            {
                return Unauthorized();
            }
            var examInstances = _repository.GetUsersExamInstances(user);
            
            return Ok(_mapper.Map<IEnumerable<ExamInstanceListReadDto>>(examInstances));
            //return Ok(examInstances);
        }

        //GET api/examinstances/:id
        [HttpGet("{id}")]
        public ActionResult<ExamInstanceAttemptReadDto> GetExamInstanceById([ModelBinder(typeof(HashIdModelBinder))] int id)
        {
            // Get user id from JWT token
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            // check that user exists
            var user = _repository.GetUserById(userId);
            if (user == null)
            {
                return Unauthorized();
            }
            
            var examInstance = _repository.GetExamInstanceById(id);
            if (examInstance == null)
            {
                return NotFound();
            }
            // make sure this is the users instance
            if (examInstance.User != user)
            {
                return Forbid();
            }
            // make sure exam instance is not already complete
            if(examInstance.IsCompleted)
            {
                return BadRequest("Exam has already been completed");
            }
            
            return Ok(_mapper.Map<ExamInstanceAttemptReadDto>(examInstance));
        }

        //GET api/examinstances/results/:id
        [HttpGet("results/{id}")]
        public ActionResult<ExamInstanceResultDto> GetExamInstanceResultsById([ModelBinder(typeof(HashIdModelBinder))] int id)
        {
            // Get user id from JWT token
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            // check that user exists
            var user = _repository.GetUserById(userId);
            if (user == null)
            {
                return Unauthorized();
            }
            System.Console.WriteLine("the id is: "+ id);
            var examInstance = _repository.GetExamInstanceById(id);
            if (examInstance == null)
            {
                return NotFound();
            }
            // make sure exam instance is complete
            if(!examInstance.IsCompleted)
            {
                return NotFound();
            }
            // make sure this is the users instance or user is authorized to view other peoples results
            if (examInstance.User != user)
            {
                if(user.Role != UserRole.ADMIN && user.Role != UserRole.TEACHER)
                {
                    return Forbid();
                }  
            }
            
            return Ok(_mapper.Map<ExamInstanceResultDto>(examInstance));
        }

        //POST api/examinstances/:id
        [HttpPost("{id}")]
        public ActionResult<ExamInstanceAttemptTurnInDto> TurnInExamInstance([ModelBinder(typeof(HashIdModelBinder))] int id, ExamInstanceAttemptTurnInDto examInstanceTurnInDto)
        {
            // Get user id from JWT token
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            // check that user exists
            var user = _repository.GetUserById(userId);
            if (user == null)
            {
                return Unauthorized();
            }
            // check that exam instance exists
            var examInstance = _repository.GetExamInstanceById(id);
            if (examInstance == null)
            {
                return NotFound();
            }
            // make sure this is the users instance
            if (examInstance.User != user)
            {
                return Forbid();
            }
            // if exam instance is already complete accept no further turn ins
            if(examInstance.IsCompleted)
            {
                return BadRequest("Exam has already been completed");
            }
            // make sure turn in is on time
            // add extra 5 second window to account for any delays in transfer etc.
            if(examInstance.Exam.EndDate.AddSeconds(10) < DateTime.UtcNow)
            {
                return BadRequest("Cannot turn in past exam end date");
            }


            // debug logging
            System.Console.WriteLine("received: " +  JsonConvert.SerializeObject(examInstanceTurnInDto));
            
            // set the exam instance answers from dto and evaluate results
            examInstance.EvaluateExamAttempt(examInstanceTurnInDto);
            _repository.SaveChanges();


            return NoContent();
        }

    }
}