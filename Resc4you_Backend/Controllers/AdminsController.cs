using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        // GET: api/<AdminsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AdminsController>/5
        [HttpGet("/GetUser/{phone}")]
        public IActionResult GetUser(string phone)//check if user exists to open equest from web
        {
            try
            {
                Admin admin = new Admin();
                return Ok(admin.GetUser(phone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("/GetVolunteerData")]
        public IActionResult GetVolunteerData()//get volunteer data for volunteers page admin web
        {
            try
            {
                Admin admin = new Admin();
                return Ok(admin.GetVolunteerData());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        // POST api/<RequestsController>
        [HttpPost("/SendNotificationsForRelevantVolunteersFromWeb/{address}/pushToken/{pushToken}")]
        public IActionResult SendNotificationsForRelevantVolunteersFromWeb(string address,string pushToken)//send push notification from web
        {
            try
            {
                Admin admin = new Admin();
                return Ok(admin.GetPushTokensForRelevantVolunteersFromWeb(address, pushToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("/SendCancelNotificationsForVolunteerFromWeb/{requestId}")]
        public IActionResult SendCancelNotificationsForVolunteerFromWeb(int requestId)//send cancel notification to volunteer from web
        {
            try
            {
                Admin admin = new Admin();
                return Ok(admin.SendCancelNotificationsForVolunteerFromWeb(requestId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT api/<AdminsController>/5
        [HttpPut("/UpdateSpecialtyActive/{specialtyId}/status/{isActive}")]
        public IActionResult UpdateSpecialtyActive(int specialtyId, bool isActive)//update specialty is active status
        {
            try
            {
                Admin admin = new Admin();
                return Ok(admin.UpdateSpecialtyActive(specialtyId,isActive));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<AdminsController>/5
        [HttpPut("/UpdateWorkerActive/{phone}/status/{isActive}")]
        public IActionResult UpdateWorkerActive(string phone, bool isActive)//update worker is active status
        {
            try
            {
                Admin admin = new Admin();
                return Ok(admin.UpdateWorkerActive(phone, isActive));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<AdminsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
