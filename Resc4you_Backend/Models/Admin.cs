using System.Reflection;

namespace Resc4you_Backend.Models
{
    public class Admin
    {

        public bool GetUser(string phone)
        {
            DBservicesAdmin admin = new DBservicesAdmin();
            return admin.GetUser(phone);
        }
        public object GetVolunteerData()
        {
            DBservicesAdmin admin = new DBservicesAdmin();
            return admin.GetVolunteerData();
        }
        public int GetPushTokensForRelevantVolunteersFromWeb(string address, string pushToken)
        {
            ExpoPushNotification e = new ExpoPushNotification();
            string notificationTitle = "New Request";
            string notificationBody = "A new request has been received at " + address + ". We will be happy to assist";
            string someData = "Available Requests";
            e.SendPushNotification(pushToken, notificationTitle, notificationBody, someData);
            return 1;

        }
        public int UpdateSpecialtyActive(int specialtyId,bool isActive)
        {
            DBservicesAdmin admin = new DBservicesAdmin();
            return admin.UpdateSpecialtyActive(specialtyId,isActive);
        }
        public int UpdateWorkerActive(string phone, bool isActive)
        {
            DBservicesAdmin admin = new DBservicesAdmin();
            return admin.UpdateWorkerActive(phone, isActive);
        }
        public int SendCancelNotificationsForVolunteerFromWeb(int requestId)
        {
            DBservicesAdmin dbs = new DBservicesAdmin();
            object response = dbs.SendCancelNotificationsForVolunteerFromWeb(requestId);
            Type type = response.GetType();

            PropertyInfo fNameProperty = type.GetProperty("fName");
            PropertyInfo lNameProperty = type.GetProperty("lName");
            PropertyInfo tokenProperty = type.GetProperty("expo_push_token");
            string fname = (string)fNameProperty.GetValue(response);
            string lname = (string)lNameProperty.GetValue(response);
            string token = (string)tokenProperty.GetValue(response);

            ExpoPushNotification e = new ExpoPushNotification();
            string notificationTitle = "Request was canceled";
            string notificationBody = "we are sorry, " + fname + " " + lname + " canceled his request";
            string someData = "Volunteer Availablity Status";
            e.SendPushNotification(token, notificationTitle, notificationBody, someData);
            return 1;

        }
    }
}
