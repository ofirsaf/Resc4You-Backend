using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        // GET: api/<WorkersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<WorkersController>/5
        [HttpGet("/GetWorkersDetails")]
        public IActionResult GetWorkersDetails()//get worker details for admin web
        {
            try
            {
                Worker worker = new Worker();
                return Ok(worker.GetWorkersDetails());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<WorkersController>
        [HttpPost("/LoginWorker")]
        public ActionResult login([FromBody] Worker worker)//login worker for web login page
        {
            ActionResult result = worker.loginWorker(worker.Phone, worker.Password);
            return result;
        }

        [HttpPost("/RegisterWorker")]
        public IActionResult RegisterWorker([FromBody] Worker worker)//register worker for admin web
        {
            try
            {
                return Ok(worker.Insert());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<WorkersController>/5
        [HttpPut]
        public IActionResult Put([FromBody] Worker worker)//update worker data for admin web
        {
            try
            {
                return Ok(worker.Update());
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<WorkersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
