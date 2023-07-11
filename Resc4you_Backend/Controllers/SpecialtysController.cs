using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtysController : ControllerBase
    {
        // GET: api/<SpecialtysController>
        [HttpGet]
        public IActionResult Get()//get all specialties for volunteer register and update pages and for request A page
        {
            try
            {
                Specialty s = new Specialty();
                return Ok(s.Read());
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET api/<SpecialtysController>/5
        [HttpGet("{volunteerPhone}")]
        public IActionResult GetVolunteerSpecialty(string volunteerPhone)//get volunteer specialties for update details page
        {
            try
            {
                Specialty specialty = new Specialty();
                return Ok(specialty.ReadVolunteerSpecialty(volunteerPhone));
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<SpecialtysController>
        [HttpPost("/insertNewSpecialty")]
        public IActionResult insertNewSpecialty([FromBody] Specialty specialty)//adding new specialty update list page web
        {
            try
            {
                return Ok(specialty.Insert());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // PUT api/<SpecialtysController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SpecialtysController>/5
        [HttpDelete("{volunteerPhone}")]
        public IActionResult Delete(string volunteerPhone)//delete specialties of volunteer for update details page
        {
            try
            {
                Specialty specialty = new Specialty();
                return Ok(specialty.DeletePersonSpecialty(volunteerPhone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
