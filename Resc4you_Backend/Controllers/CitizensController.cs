using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitizensController : ControllerBase
    {
        // GET: api/<CitizensController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{phone}")]
        public IActionResult GetCitizen(string phone)//get the user data for update details
        {
            Citizen volunteer = new Citizen();
            List<Citizen> tmp = volunteer.ReadCitizen(phone);
            if (tmp.Count() > 0)
            {
                return Ok(tmp[0]);
            }
            return NotFound();
        }

        // GET api/<CitizensController>/5
        [HttpGet("/phone/{phone}")]
        public IActionResult GetCitizenRequest(string phone)//get all the requests of the citizen
        {
            try
            {
                Citizen citizen = new Citizen();
                return Ok(citizen.getCitizenRequest(phone));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET api/<CitizensController>/5
        [HttpGet("/GetCitizenOpenRequest/{phone}")]
        public IActionResult GetCitizenOpenRequest(string phone)//get the open request of the user to listen to status on firebase
        {
            try
            {
                Citizen citizen = new Citizen();
                return Ok(citizen.GetCitizenOpenRequest(phone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET api/<CitizensController>/5
        [HttpGet("/GetDetailsOfReportedRequest/{phone}")]
        public IActionResult GetDetailsOfReportedRequest(string phone)//get details of open request for status A page
        {
            try
            {
                Citizen citizen = new Citizen();
                return Ok(citizen.GetDetailsOfReportedRequest(phone));
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<CitizensController>/5
        [HttpGet("/NumberCitizenOpenRequest/{phone}")]
        public IActionResult GetNumberCitizenOpenRequest(string phone)//check if the citizen has open request or not for drawer
        {
            try
            {
                Citizen citizen = new Citizen();
                return Ok(citizen.GetNumberCitizenOpenRequest(phone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST api/<CitizensController>
        [HttpPost]
        public IActionResult Post([FromBody] Citizen citizen)//register new citizen
        {
            try
            {
                return Ok(citizen.Insert());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/CitizenPhone/{CitizenPhone}/CitizenLanguageId/{CitizenLanguageId}")]
        public IActionResult Post(string CitizenPhone,int CitizenLanguageId)//save citizen languages
        {
            try
            {
                Citizen citizen = new Citizen();
                return Ok(citizen.InsertCitizenLanguage(CitizenPhone, CitizenLanguageId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




        // PUT api/<CitizensController>/5
        [HttpPut]
        public IActionResult Put([FromBody] Citizen citizen)//update citizen data
        {
            try
            {
                return Ok(citizen.Update());
            }

            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<CitizensController>/5
        [HttpDelete("{requestId}")]
        public IActionResult Delete(int requestId)//delete request in case of citizen doesnt need help
        {
            try
            {
                Citizen citizen = new Citizen();
                return Ok(citizen.DeleteRequest(requestId));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
