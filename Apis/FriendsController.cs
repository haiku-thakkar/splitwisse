using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondSplitWise.DBContext;
using SecondSplitWise.Model;
using SecondSplitWise.Repository;
using SecondSplitWise.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecondSplitWise.Apis
{
    [Produces("application/json")]
    [Route("api/friends")]
    public class FriendsController : Controller
    {
        IFriendRepository _FriendRepository;
        ILogger _Logger;
        private SecondSplitWiseContext _Context;

        public FriendsController(IFriendRepository friendRepo, ILoggerFactory loggerFactory, SecondSplitWiseContext context)
        {
            _FriendRepository = friendRepo;
            _Logger = loggerFactory.CreateLogger(nameof(FriendsController));
            _Context = context;
        }

        // GET api/friends/id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FriendResponse), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> Friend(int id)
        {
            try
            {
                var friends = await _FriendRepository.GetFriendAsync(id);
                return Ok(friends);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

        // GET api/friends/all/id
        [HttpGet("all/{id}")]
        [ProducesResponseType(typeof(FriendResponse), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> Friends(int id)
        {
            try
            {
                var friends = await _FriendRepository.GetAllFriendsAsync(id);
                return Ok(friends);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }


        // POST api/friends/id(user)/username(friend)/email(friend)
        [HttpPost("{id}/{userName}/{email}")]
        [ProducesResponseType(typeof(ApiGeneralResponse), 201)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> CreateFriend(int id, string userName, string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiGeneralResponse { Status = false });
            }

            try
            {
                var newUser = await _FriendRepository.InsertFriendAsync(id,userName,email);
                if (newUser == null)
                {
                    return BadRequest(new ApiGeneralResponse { Status = false });
                }
                if (newUser.UserId != 0)
                {
                    return CreatedAtAction("GetFriendRoute", new { id = newUser.UserId },
                           new ApiGeneralResponse { Status = true, id = newUser.FriendListId });
                }
                else
                {
                    return BadRequest( new ApiGeneralResponse { Status = false, id = 0 });
                }
                        
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }


        // DELETE api/friends/uid/fid
        [HttpDelete("{uid}/{fid}")]
        [ProducesResponseType(typeof(ApiGeneralResponse), 200)]
        [ProducesResponseType(typeof(ApiGeneralResponse), 400)]
        public async Task<ActionResult> DeleteUser(int fid, int uid)
        {
            try
            {
                var status = await _FriendRepository.DeleteFriendAsync(fid,uid);
                if (!status)
                {
                    return BadRequest(new ApiGeneralResponse { Status = false });
                }
                return Ok(new ApiGeneralResponse { Status = true, id =fid });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiGeneralResponse { Status = false });
            }
        }

    }
}
