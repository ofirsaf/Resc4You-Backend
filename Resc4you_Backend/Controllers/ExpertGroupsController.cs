using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertGroupsController : ControllerBase
    {
        // GET: api/<ExpertGroupsController>
        [HttpGet]
        public IActionResult Get()//get expert groups for volunteer registeration and update details pages
        {
            try
            {
                ExpertGroup expertGroup = new ExpertGroup();
                return Ok(expertGroup.Read());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        // GET api/<ExpertGroupsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ExpertGroupsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ExpertGroupsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ExpertGroupsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
