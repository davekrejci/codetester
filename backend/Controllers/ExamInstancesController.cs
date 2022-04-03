using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
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
        public ActionResult<IEnumerable<Exam>> GetAllExamInstances()
        {
            var examInstances = _repository.GetAllExamInstances();
            
            //return Ok(_mapper.Map<IEnumerable<ExamInstanceReadDto>>(exams));
            return Ok(examInstances);
        }

        //GET api/examinstances
        [HttpGet]
        public ActionResult<IEnumerable<Exam>> GetUsersExamInstances()
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
        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Exam>> GetExamInstanceById(int id)
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

            // make sure this is the users instance
            
            
            //return Ok(_mapper.Map<IEnumerable<ExamInstanceReadDto>>(exams));
            return Ok(_mapper.Map<ExamInstanceAttemptReadDto>(examInstance));
        }

    }
}