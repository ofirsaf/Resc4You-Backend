using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Resc4you_Backend.Models
{
    public class ExpoPushNotification
    {

        public async void SendPushNotification(string token,string notificationTitle,string notificationBody,string someData)
        {
            string expoPushToken = token;

            var message = new
            {
                to = expoPushToken,
                sound = "default",
                title = notificationTitle,
                body = notificationBody,
                data = new { screen=someData }
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

                }
            }
        }

        public async void SendPushNotification(string token, string notificationTitle, string notificationBody)
        {
            string expoPushToken = token;

            var message = new
            {
                to = expoPushToken,
                sound = "default",
                title = notificationTitle,
                body = notificationBody,
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

                }
            }
        }
    }
}
