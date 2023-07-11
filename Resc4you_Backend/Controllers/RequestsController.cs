using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Resc4you_Backend.Models;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Timers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private static readonly Dictionary<int, System.Timers.Timer> requestsTimers = new Dictionary<int, System.Timers.Timer>();

        // GET: api/<RequestsController>
        [HttpGet("/GetAllRequestForAdmin")]
        public IActionResult GetAllRequestForAdmin()//get all requests in db for admin
        {
            try
            {
                Request request = new Request();
                return Ok(request.getAllRequest());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET api/<RequestsController>/5
        [HttpGet("/GetRelevantVolunteer/{specialtyId}")]
        public IActionResult GetRelevantVolunteer(int specialtyId)//get all the available volunteers with the same specailty of the request after citizen open new request
        {
            try
            {
                Request request = new Request();
                return Ok(request.GetRelevantVolunteer(specialtyId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/GetRelevantRequestsToVolunteer/{VolunteerPhone}")]
        public IActionResult GetRelevantRequestsToVolunteer(string VolunteerPhone)//get all the relevant requests to volunteer in avilable requests page
        {
            try
            {
                Request request = new Request();
                return Ok(request.GetRelevantRequestsToVolunteer(VolunteerPhone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/GetVolunteerHandlePushTokken/{requestId}")]
        public IActionResult GetVolunteerHandlePushTokken(int requestId)//get the pust token of the volunteer handle specific request
        {
            try
            {
                Request request = new Request();
                return Ok(request.GetVolunteerHandlePushTokken(requestId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/RequestAlreadyTaken/{requestId}")]
        public IActionResult RequestAlreadyTaken(int requestId)//check if another volunteer took the request
        {
            try
            {
                Request request = new Request();
                return Ok(request.RequestAlreadyTaken(requestId));
            }
          
             catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/GetRequestAndVolunteerLocation/{requestId}/volunteerPhone/{phone}")]
        public IActionResult GetRequestAndVolunteerLocation(int requestId,string phone)//get request and volunteer location for status B page
        {
            try
            {
                Request request = new Request();
                return Ok(request.GetRequestAndVolunteerLocation(requestId, phone));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/GetDetailsOfCitizenOpenedRequest/{requestId}")]
        public IActionResult GetDetailsOfCitizenOpenedRequest(int requestId)//Get Details Of Citizen Opened Request for updating request in admin page
        {
            try
            {
                Request request = new Request();
                return Ok(request.GetDetailsOfCitizenOpenedRequest(requestId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<RequestsController>
        [HttpPost]
        public IActionResult Post([FromBody] Request request)//insert new request
        {
            try
            {
                int requestId = request.Insert();
                System.Threading.Timer timer = new System.Threading.Timer(TimerCallback, requestId, 3600000, Timeout.Infinite);//after 60 minutes
                return Ok(requestId);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }
        // POST api/<RequestsController>
        [HttpPost("/InsertToVolunteerOfRequest/{phone}/RequestId/{requestId}")]
        public IActionResult InsertToVolunteerOfRequest(string phone,int requestId)//insert all the relevant volunteers to volunteer of request table
        {
            try
            {
                Request request = new Request();
                return Ok(request.InsertToVolunteerOfRequest(phone, requestId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // PUT api/<RequestsController>/5
        [HttpPut("/UpdateRateOfRequest/{requestId}/Review/{review}/Rating/{rating}")]
        public IActionResult Put(int requestId, string review, int rating)//update request review and rating
        {
            try
            {
                Request request = new Request();
                return Ok(request.UpdateRateOfRequest(requestId, review, rating));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<RequestsController>/5
        [HttpPut("/UpdateRequestStatusToClose/{requestId}/Phone/{phone}/address/{address}/long/{longi}/lat/{lati}")]
        public IActionResult UpdateRequestStatusToClose(int requestId, string phone, string address,double longi,double lati)//update request status to closed and update volunteer location after closing the request
        {
            try
            {
                Request request = new Request();
                return Ok(request.UpdateRequestStatusToClose(requestId, phone, address, longi, lati));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<RequestsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        private void TimerCallback(object requestId)//60 minutes after isert the reqest
        {
            int id = (int)requestId;
            Request r=new Request();
            object response = r.GetStatusOfSpecificRequest(id);//chech the status of the request

            Type type = response.GetType();

            PropertyInfo statusProperty = type.GetProperty("requestStatus");
            PropertyInfo tokenProperty = type.GetProperty("expo_push_token");

            if (statusProperty != null && tokenProperty != null)
            {
                string requestStatus = (string)statusProperty.GetValue(response);
                string token = (string)tokenProperty.GetValue(response);

                if (requestStatus == "Waiting")//in case no volunteer took the request
                {
                    Citizen citizen = new Citizen();
                    citizen.DeleteRequest(id);//delete the request
                    ExpoPushNotification e = new ExpoPushNotification();
                    string notificationTitle = "Update for your request";
                    string notificationBody = "we are sorry but there isnt an available volunteer, your request is closed";
                    e.SendPushNotification(token,notificationTitle,notificationBody);//send a notification for the citizen
                }
            }
            else
            {
                // Handle the case where either the "requestStatus" or "token" property is missing
            }


        }



    }
}
