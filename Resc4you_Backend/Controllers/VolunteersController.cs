using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersController : ControllerBase
    {
        // GET: api/<VolunteersController>
        [HttpGet]
        public IActionResult Get()//get all volunteers for filters in requests page admin
        {
            try
            {
                Volunteer volunteer = new Volunteer();
                return Ok(volunteer.GetAllVolunteers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<VolunteersController>/5
        [HttpGet("{phone}")]
        public IActionResult GetVolunteer(string phone)//get the user data for update details
        {
            Volunteer volunteer = new Volunteer();
            List<Volunteer> tmp = volunteer.ReadVolunteer(phone);
            if(tmp.Count()>0)
            {
                return Ok(tmp[0]);
            }
            return NotFound();
        }

        // GET api/<VolunteersController>/5
        [HttpGet("/VolunteerPhone/{VolunteerPhone}")]
        public IActionResult GetVolunteerRelevantRequest(string VolunteerPhone)//get the relevant requests for volunteer to show them on available requests page
        {
            try
            {
                Volunteer volunteer = new Volunteer();
                return Ok(volunteer.GetVolunteerRelevantRequest(VolunteerPhone));
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET api/<VolunteersController>/5
        [HttpGet("/VolunteerHandleRequest/{VolunteerPhone}")]
        public IActionResult GetVolunteerHandleRequest(string VolunteerPhone)//get list of al the requets handled by volunteer to show them on my request volunteer page
        {
            try
            {
                Volunteer volunteer = new Volunteer();
                return Ok(volunteer.GetVolunteerHandleRequest(VolunteerPhone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/checkNumRequestHandling/{VolunteerPhone}")]
        public IActionResult GetHandelingRequestsVolunteer(string VolunteerPhone)//check if volunteer already handled another request
        {
            try
            {
                Volunteer volunteer = new Volunteer();
                return Ok(volunteer.HandelingRequestsVolunteer(VolunteerPhone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
   
        }

        [HttpGet("/getPushTokensForChat/{expertiseId}/phone/{phone}")]
        public IActionResult GetPushTokensForChat(int expertiseId, string phone)//get push token of available volunteers on specific expertise
        {
            try
            {
                Volunteer volunteer = new Volunteer();
                return Ok(volunteer.GetPushTokensForChat(expertiseId, phone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

            // POST api/<VolunteersController>
            [HttpPost]
        public IActionResult Post([FromBody] Volunteer volunteer)//register new volunteer
        {
            try
            {
                return Ok(volunteer.Insert());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/VolunteerPhone/{VolunteerPhone}/VolunteerLanguageId/{VolunteerLanguageId}")]
        public IActionResult PostLanguage(string VolunteerPhone, int VolunteerLanguageId)//save volunteer languages
        {
            try
            {
                Volunteer volunteer = new Volunteer();
                return Ok(volunteer.InsertVolunteerLanguage(VolunteerPhone, VolunteerLanguageId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("/VolunteerPhone/{VolunteerPhone}/VolunteerSpecialtyId/{VolunteerSpecialtyId}")]
        public IActionResult PostSpecialty(string VolunteerPhone, int VolunteerSpecialtyId)//save volunteer specialties
        {
            try
            {
                Volunteer volunteer = new Volunteer();
                return Ok(volunteer.InsertVolunteerSpecialty(VolunteerPhone, VolunteerSpecialtyId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }


        [HttpPut("/UpdateLocation/{VolunteerPhone}/address/{address}/longitude/{longitude}/latitude/{latitude}")]
        public IActionResult UpdateLocation(string VolunteerPhone, string address, double longitude, double latitude)//update volunteer location
        {
            try
            {
                Volunteer volunteer = new Volunteer();
                return Ok(volunteer.UpdateLocation(VolunteerPhone, address,longitude,latitude));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT api/<VolunteersController>/5
        [HttpPut]
        public IActionResult Put([FromBody] Volunteer volunteer)//update volunteer data
        {
            try
            {
                return Ok(volunteer.Update());
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<VolunteersController>/5
        [HttpPut("/VolunteerPhone/{VolunteerPhone}/RequestId/{RequestId}")]
        public IActionResult declineRequest(string VolunteerPhone, int RequestId)//when volunteer doesnt want to handle a request
        {
            try
            {
                Volunteer volunteer = new Volunteer();
                return Ok(volunteer.declineRequest(VolunteerPhone, RequestId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<VolunteersController>/5
        [HttpPut("/VolunteerPhone/{VolunteerPhone}/RequestToHandle/{RequestId}")]
        public IActionResult AssociateRequestToVolunteer(string VolunteerPhone, int RequestId)// Associate Request To Volunteer and update request table and volunteer of request table 
        {
            try
            {
                Volunteer volunteer = new Volunteer();
                return Ok(volunteer.AssociateRequestToVolunteer(VolunteerPhone, RequestId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // PUT api/<VolunteersController>/5
        [HttpPut("/RequestToCancel/{RequestId}")]
        public IActionResult CancelRequestFromVolunteer(int RequestId)//when volunteer cancel his arriving
        {
            try
            {
                Volunteer volunteer = new Volunteer();
                return Ok(volunteer.CancelRequestFromVolunteer(RequestId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE api/<VolunteersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
