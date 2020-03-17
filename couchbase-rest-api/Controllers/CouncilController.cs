using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using couchbase_rest_api.Models;
using couchbase_rest_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace couchbase_rest_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouncilController : ControllerBase
    {
        private ICouncilService _councilService;
        private IBucket _bucket;

        public CouncilController(ICouncilService councilService)
        {
            _bucket = ClusterHelper.GetBucket("lawyermanagementdb");
            _councilService = councilService;

        }

        [HttpGet]
        [Route("GetAllCouncils")]
        public IActionResult GetAll()
        {
            var councils = _councilService.GetAll();
            return Ok(councils);
        }
        [HttpGet]
        [Route("GetCouncilByUser")]
        public IActionResult GetCouncilByUser(Guid userId)
        {
            var councils = _councilService.GetByUser(userId);
            return Ok(councils);
        }


        [HttpPost]
        [Route("AddNewCouncil")]
        public IActionResult AddNewCouncil([FromBody] Council council)
        {
            if (!council.Id.HasValue)
            {
                council.Id = Guid.NewGuid();

                _bucket.Upsert(council.Id.ToString(), new
                {
                    council.UserId,
                    council.Name,
                    council.PhysicalAddress,
                    council.DateCreated,
                    Type = "Council"
                });
            }
            return Ok(council);
        }

        [HttpDelete]
        [Route("DeleteCouncil{id}")]
        public IActionResult DeleteCouncil (Guid id)
        {
            var result = _bucket.Remove(id.ToString());
            return Ok(id);
        }
    }
}