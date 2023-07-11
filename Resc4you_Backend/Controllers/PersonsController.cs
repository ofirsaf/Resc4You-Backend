using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        // GET: api/<PersonsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PersonsController>/5
        [HttpGet("/GetUserDetails/{phone}")]
        public IActionResult GetUserDetails(string phone)//get user details for case when there is a token to the user and he doesnt have to register again
        {
            try
            {
                Person person = new Person();
                return Ok(person.GetUserDetails(phone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
        }

        // POST api/<PersonsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPost("/login")]
        public ActionResult login([FromBody] Person person)//login person
        {
            ActionResult result = person.loginUser(person.Phone, person.Password);
            return result;
        }
        // PUT api/<PersonsController>/5
        [HttpPut("/UpdateUserPushNotificationToken/{phone}/PushNotificationToken/{pushNotificationToken}")]
        public IActionResult UpdateUserPushNotificationToken(string phone, string pushNotificationToken)//update person push notification token when after log in
        {
            try
            {
                Person p = new Person();
                return Ok(p.UpdateUserPushNotificationToken(phone, pushNotificationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

         // PUT api/<PersonsController>/5
        [HttpPut("/UpdateIsActive/{phone}/status/{isActive}")]
        public IActionResult UpdateIsActive(string phone, bool isActive)//update volunteer is active status
        {
            try
            {
                Person p = new Person();
                return Ok(p.UpdateIsActive(phone, isActive));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // DELETE api/<PersonsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
