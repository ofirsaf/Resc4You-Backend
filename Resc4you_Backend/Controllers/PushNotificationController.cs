using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushNotificationController : ControllerBase
    {
        // GET: api/<PushNotificationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PushNotificationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PushNotificationController>
        [HttpPost("/SendPushNotification/{token}")]
        public async Task<IActionResult> SendPushNotification(string token)//controller for sending push token right now not in use we are sendinf the push notification through specific controllers
        {
            string expoPushToken = token;
            string notificationTitle = "Update for your request";
            string notificationBody = "we are sorry but there isnt an available volunteer, your request is closed";
            string someData = "Some data goes here";
            
            var message = new
            {
                to = expoPushToken,
                sound = "default",
                title = notificationTitle,
                body = notificationBody,
                data = new { someData }
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Accept-encoding", "gzip, deflate");
                httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US");

                var json = JsonConvert.SerializeObject(message);

                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync("https://exp.host/--/api/v2/push/send", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode);
                    }
                }
            }
        }

        // PUT api/<PushNotificationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PushNotificationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
