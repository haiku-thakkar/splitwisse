using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSplitWise.DBContext;
using SecondSplitWise.Model;
using SecondSplitWise.Repository;
using SecondSplitWise.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SecondSplitWise.Apis
{
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : Controller
    {
        IUserRepository _UserRepository;
        ILogger _Logger;
        private SecondSplitWiseContext _Context;

        public UserController(IUserRepository userRepo, ILoggerFactory loggerFactory, SecondSplitWiseContext context)
        {
            _UserRepository = userRepo;
            _Logger = loggerFactory.CreateLogger(nameof(UserController));
            _Context = context;
        }
        

        // GET api/user
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> Users()
        {

            try
            {
                var Users = await _UserRepository.GetUsersAsync();
                return Ok(Users);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // GET api/user/id
        [HttpGet("{id}", Name = "GetUserRoute")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> Users(int id)
        {
            try
            {
                var user = await _UserRepository.GetUserAsync(id);
                return Ok(user);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // POST api/user
        [HttpPost]
        [ProducesResponseType(typeof(ApiGeneralResponse), 201)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> CreateUser([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiGeneralResponse { Status = false });
            }

            try
            {
                var newUser = await _UserRepository.InsertUserAsync(user);
                if (newUser == null)
                {
                    return BadRequest(new ApiGeneralResponse { Status = false });
                }
                return CreatedAtRoute("GetUserRoute", new { id = newUser.UserId },
                        new ApiGeneralResponse { Status = true, id = newUser.UserId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // PUT api/user/id
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiGeneralResponse), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> UpdateUser(int id, [FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiGeneralResponse { Status = false });
            }

            try
            {
                var status = await _UserRepository.UpdateUserAsync(user);
                if (!status)
                {
                    return BadRequest(new ApiGeneralResponse { Status = false });
                }
                return Ok(new ApiGeneralResponse { Status = true, id = user.UserId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // DELETE api/user/id
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiGeneralResponse), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var status = await _UserRepository.DeleteUserAsync(id);
                if (!status)
                {
                    return BadRequest(new ApiGeneralResponse { Status = false });
                }
                return Ok(new ApiGeneralResponse { Status = true, id = id });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        //GET api/user/login/email/password
        [Route("api/user/login/{email}/{password}")]
        [HttpGet("login/{email}/{password}")]
        [ProducesResponseType(typeof(ApiGeneralResponse), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> Loginuser(string email, string password)
        {
            try
            {
                var user = await _UserRepository.LoginUserAsync(email,password);
                if (user== null)
                {
                    return BadRequest(new ApiGeneralResponse { Status = false });
                }
                return Ok(new ApiGeneralResponse { Status = true,id= user.UserId });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }

        }
    }
}
