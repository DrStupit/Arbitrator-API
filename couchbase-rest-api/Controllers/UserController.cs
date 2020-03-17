using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;
using couchbase_rest_api.Models;
using couchbase_rest_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace couchbase_rest_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IBucket _bucket;
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _bucket = ClusterHelper.GetBucket("users");
            _userService = userService;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("Get/{id}")]
        public IActionResult Get(Guid id)
        {
            var result = _bucket.Get<User>(id.ToString());
            return Ok(result.Value);
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult AddNewUser([FromBody] User user)
        {
            if (!user.Id.HasValue)
            {
                user.Id = Guid.NewGuid();

                _bucket.Upsert(user.Id.ToString(), new
                {
                    user.Name,
                    user.LastName,
                    user.Email,
                    user.CellNo,
                    user.DateCreated,
                    user.Password,
                    Type = "User"
                });
            }
            return Ok(user);
        }

        [HttpDelete]
        [Route("Delete{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            var result = _bucket.Remove(id.ToString());
            return Ok(id);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Authenticate([FromBody]Authenticate model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);
            if (user == null)
                return BadRequest(new { message = "Email or password is incorrect" });
            HttpContext.Session.SetString("SessionToken", user.Token);
            return Ok(user);
        }

    }
}