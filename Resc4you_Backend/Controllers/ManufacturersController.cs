using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        // GET: api/<ManufacturersController>
        [HttpGet]
        public IActionResult Get()//get list of manufacturers for request C page
        {
            try
            {
                Manufacturer m = new Manufacturer();
                return Ok(m.Read());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ManufacturersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManufacturersController>
        [HttpPost("/insertNewManufacturer")]
        public IActionResult insertNewManufacturer([FromBody] Manufacturer manufacturer)//adding new manufacturer update list page web
        {
            try
            {
                return Ok(manufacturer.Insert());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ManufacturersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ManufacturersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
