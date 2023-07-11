using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Timers;

namespace Resc4you_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimerController : ControllerBase
    {
        private static readonly Dictionary<string, System.Timers.Timer> timers = new Dictionary<string, System.Timers.Timer>();
        private static readonly Dictionary<string, int> elapsedMinutes = new Dictionary<string, int>();


        [HttpPut("/minutes/{minutes}/hours/{hours}/phone/{phone}/avilabilityStatus/{avilabilityStatus}/address/{address}/longitude/{longitude}/latitude/{latitude}")]
        public ActionResult StartTimer(int minutes, int hours, string phone, bool avilabilityStatus,string address, double longitude, double latitude)
        {
            int totalTime = minutes + hours * 60;
            Volunteer v = new Volunteer();
          
            if(avilabilityStatus==false)//in case avilability status is false
            {
                v.ResetVolunteerStatus(0, 0, phone, false, address, longitude, latitude);//update volunteer status and delete the requests from volunteer of request table

            }
            else//in case avilability status is true
            {
                v.UpdateStatus(minutes, hours, phone, avilabilityStatus, address, longitude, latitude);//update volunteer status
                v.updateVolunteerPressTime(DateTime.Now, phone);//update volunteer press time

            }

            // If a timer is already running for this phone number, stop it for case when volunteer change his availabilty to false before timer end and want to create new timer
            if (timers.ContainsKey(phone))
            {
                System.Timers.Timer oldTimer = timers[phone];
                oldTimer.Enabled = false;
                oldTimer.Dispose();
                timers.Remove(phone);
                elapsedMinutes.Remove(phone);
            }

            // Create a new timer for this phone number
            System.Timers.Timer newTimer = new System.Timers.Timer(60000);
            newTimer.Elapsed += (sender, e) => OnTimedEvent(sender, e, totalTime, phone, address, longitude, latitude);//every minutes passed
            newTimer.Interval = 60000;
            newTimer.Enabled = true;
            timers[phone] = newTimer;
            elapsedMinutes[phone] = 0;

            return Ok();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e, int totalTime, string phone, string address, double longitude, double latitude)
        {
            // Get the elapsed time for this phone number
            int currentElapsedMinutes = 0;
            elapsedMinutes.TryGetValue(phone, out currentElapsedMinutes);

            // Increment the elapsed time by one minute
            currentElapsedMinutes++;

            if (currentElapsedMinutes >= totalTime)//in case timer finished
            {
                // Stop the timer
                System.Timers.Timer timer = timers[phone];
                timer.Enabled = false;
                timer.Dispose();
                timers.Remove(phone);
                elapsedMinutes.Remove(phone);

                // Call the GoToServer function
                Volunteer v = new Volunteer();
                v.ResetVolunteerStatus(0, 0, phone, false, "", 0, 0);//update volunteer status and delete the requests from volunteer of request table
            }
            else//in case timer didnt finish
            {
                // Update the status of the volunteer in the database
                int remainingTime = totalTime - currentElapsedMinutes;
                int remainingHours = remainingTime / 60;
                int remainingMinutes = remainingTime % 60;
                Volunteer v = new Volunteer();
                v.UpdateStatus(remainingMinutes, remainingHours, phone, true);//each minute passes update the time in db
                elapsedMinutes[phone] = currentElapsedMinutes;
            }
        }

    }
}
