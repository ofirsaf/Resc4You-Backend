using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        // GET: api/<LanguagesController>
        [HttpGet]
        public IActionResult Get()//get languages for registeration and update pages
        {
            try
            {
                Language language = new Language();
                return Ok(language.Read());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET api/<LanguagesController>/5
        [HttpGet("{personPhone}")]
        public IActionResult GetPersonLangugae(string personPhone)//get the languages of person for update details page
        {
            try
            {
                Language language = new Language();
                return Ok(language.ReadPersonLanguge(personPhone));
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<LanguagesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LanguagesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LanguagesController>/5
        [HttpDelete("{personPhone}")]
        public IActionResult Delete(string personPhone)//delete languages of person for update details page
        {
            try
            {
                Language language = new Language();
                return Ok(language.DeletePersonLanguge(personPhone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
