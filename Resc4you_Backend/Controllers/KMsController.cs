using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KMsController : ControllerBase
    {      

        // GET api/<KMsController>/5
        [HttpGet]
        public IActionResult GetKM()//get km distance
        {
            try
            {
                KM distance = new KM();
                return Ok(distance.GetKM());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST api/<KMsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<KMsController>/5
        [HttpPut]
        public IActionResult Put([FromBody] KM distance)//update km distance
        {
            try
            {
                return Ok(distance.Update());
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<KMsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
