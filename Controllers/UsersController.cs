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

            // check that role is one of possible roles or empty
            if (userCreateDto.Role != null && userCreateDto.Role != "Student" && userCreateDto.Role != "Teacher" && userCreateDto.Role != "Admin")
            {
                return BadRequest("Role " + userCreateDto.Role + " not valid option");
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

         //POST api/users/import
        [HttpPost("import")]
        public ActionResult<ICollection<UserReadDto>> ImportUsers(ICollection<UserCreateDto> userCreateDtos)
        {
            foreach (var userCreateDto in userCreateDtos)
            {
                // check that usernames are unique
                var user = _repository.GetUserByUsername(userCreateDto.Username);
                if (user != null)
                {
                    return BadRequest("Username " + user.Username + " is already taken");
                }
                // check that role is one of possible roles or empty
                if (userCreateDto.Role != null && userCreateDto.Role != "Student" && userCreateDto.Role != "Teacher" && userCreateDto.Role != "Admin")
                {
                    return BadRequest("Role " + userCreateDto.Role + " not valid option");
                }
            }

            foreach (var userCreateDto in userCreateDtos)
            {
                var userModel = _mapper.Map<User>(userCreateDto);

                // hash password
                AuthUtil.CreatePasswordHash(userCreateDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                userModel.PasswordHash = passwordHash;
                userModel.PasswordSalt = passwordSalt;

                _repository.CreateUser(userModel);
            }
            _repository.SaveChanges();
            return NoContent();
        }

        //PUT api/users/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, UserUpdateDto userUpdateDto)
        {
            // check that username is unique
            var userModel = _repository.GetUserById(id);
            if (userModel == null)
            {
                return NotFound();
            }
            // check that role is one of possible roles or empty
            if (userUpdateDto.Role != null && userUpdateDto.Role != "Student" && userUpdateDto.Role != "Teacher" && userUpdateDto.Role != "Admin")
            {
                return BadRequest("Role " + userUpdateDto.Role + " not valid option");
            }
            _mapper.Map(userUpdateDto, userModel);
            _repository.UpdateUser(userModel);
            
            _repository.SaveChanges();
            return NoContent();
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

        // admin only route for changing any users password (eg. forgotten passwords)
        //POST api/users/password/reset/:id
        [HttpPost("password/reset/{id}")]
        public ActionResult AdminResetPassword(int id, UserAdminPasswordResetDto userAdminPasswordResetDto)
        {
            // check that user exists
            var userModel = _repository.GetUserById(id);
            if (userModel == null)
            {
                return NotFound();
            }

            // hash password
            AuthUtil.CreatePasswordHash(userAdminPasswordResetDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // update model
            userModel.PasswordHash = passwordHash;
            userModel.PasswordSalt = passwordSalt;

            // save to repository
            _repository.UpdateUser(userModel);
            _repository.SaveChanges();

            return NoContent();
        }

        // any user route for changing their own password
        //POST api/users/password/change
        [Authorize(Roles = "Admin, Teacher, Student")]
        [HttpPost("password/change")]
        public ActionResult ChangePassword(UserPasswordChangeDto userPasswordChangeDto)
        {

            // Get user id from JWT token
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            // check that user exists
            var userModel = _repository.GetUserById(userId);
            if (userModel == null)
            {
                return NotFound();
            }

            // verify current password
            var isPasswordValid = AuthUtil.VerifyPasswordHash(userPasswordChangeDto.CurrentPassword, userModel.PasswordHash, userModel.PasswordSalt);
            if(!isPasswordValid)
            {
                return BadRequest("Wrong password");
            }

            // hash new password
            AuthUtil.CreatePasswordHash(userPasswordChangeDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

            // update model
            userModel.PasswordHash = passwordHash;
            userModel.PasswordSalt = passwordSalt;

            // save to repository
            _repository.UpdateUser(userModel);
            _repository.SaveChanges();

            return NoContent();
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