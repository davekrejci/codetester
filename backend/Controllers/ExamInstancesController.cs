using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                return NotFound();
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
                return NotFound();
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
            
            return Ok(_mapper.Map<ExamInstanceAttemptReadDto>(examInstance));
        }

    }
}