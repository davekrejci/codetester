using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Codetester.Data;
using Codetester.Dtos;
using Codetester.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Codetester.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ICodetesterRepo _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public UsersController(ICodetesterRepo repository, IMapper mapper, IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
        }
        
        //GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(users));
        }

        //GET api/users/{id}
        [HttpGet("{id}", Name = "GetUserById")]
        public ActionResult<UserReadDto> GetUserById(int id)
        {
            var user = _repository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserReadDto>(user));
        }

        //POST api/users
        [HttpPost]
        public ActionResult<UserReadDto> CreateUser(UserCreateDto userCreateDto)
        {
            // check that username is unique
            var user = _repository.GetUserByUsername(userCreateDto.Username);
            if (user != null)
            {
                return BadRequest("Username " + user.Username + " is already taken");
            }

            var userModel = _mapper.Map<User>(userCreateDto);

            // hash password
            AuthUtil.CreatePasswordHash(userCreateDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            userModel.PasswordHash = passwordHash;
            userModel.PasswordSalt = passwordSalt;

            _repository.CreateUser(userModel);
            _repository.SaveChanges();

            var userReadDto = _mapper.Map<UserReadDto>(userModel);
            return CreatedAtRoute(nameof(GetUserById), new { Id = userReadDto.Id }, userReadDto);
        }

        //DELETE api/users/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _repository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            _repository.DeleteUser(user);
            _repository.SaveChanges();
            return NoContent();
        }

        //POST api/users/login
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<UserLoginReadDto> Login(UserLoginDto userLoginDto)
        {
            // look for user
            var userModel = _repository.GetUserByUsername(userLoginDto.Username);
            if (userModel == null)
            {
                return BadRequest("User not found");
            }
            // verify password
            var isPasswordValid = AuthUtil.VerifyPasswordHash(userLoginDto.Password, userModel.PasswordHash, userModel.PasswordSalt);
            if(!isPasswordValid)
            {
                return BadRequest("Wrong password");
            }
            // return user with token
            var userLoginReadDto = _mapper.Map<UserLoginReadDto>(userModel);
            userLoginReadDto.Token = CreateToken(userModel);
            return Ok(userLoginReadDto);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        

    }
}