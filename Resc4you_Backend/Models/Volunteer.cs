using System.Numerics;

namespace Resc4you_Backend.Models
{
    public class Volunteer : Person
    {
        bool avilabilityStatus;
        int hoursAvailable;
        int minsAvailable;
        DateTime pressTime;
        int expertId;
        string volunteerAddress;
        double volunteerLongitude;
        double volunteerLatitude;

        public bool AvilabilityStatus { get => avilabilityStatus; set => avilabilityStatus = value; }
        public int HoursAvailable { get => hoursAvailable; set => hoursAvailable = value; }
        public int MinsAvailable { get => minsAvailable; set => minsAvailable = value; }
        public DateTime PressTime { get => pressTime; set => pressTime = value; }
        public int ExpertId { get => expertId; set => expertId = value; }
        public string VolunteerAddress { get => volunteerAddress; set => volunteerAddress = value; }
        public double VolunteerLongitude { get => volunteerLongitude; set => volunteerLongitude = value; }
        public double VolunteerLatitude { get => volunteerLatitude; set => volunteerLatitude = value; }

        public int Insert()
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            this.Password = BCrypt.Net.BCrypt.HashPassword(this.Password);
            dbs.InsertPerson(this);
            return dbs.InsertVolunteer(this);

        }

        public int InsertVolunteerLanguage(string VolunteerPhone, int VolunteerLanguageId)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.InsertVolunteerLanguage(VolunteerPhone, VolunteerLanguageId);

        }

        public int InsertVolunteerSpecialty(string VolunteerPhone, int VolunteerSpecialtyId)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.InsertVolunteerSpecialty(VolunteerPhone, VolunteerSpecialtyId);

        }

        public List<Volunteer> ReadVolunteer(string phone)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.ReadVolunteer(phone);
        }

        public int Update()
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            this.Password = BCrypt.Net.BCrypt.HashPassword(this.Password);
            dbs.UpdatePerson(this);
            return dbs.UpdateVolunteer(this);

        }

        public void UpdateStatus(int min, int hours, string phone,bool avilabilityStatus, string address, double longitude, double latitude)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            dbs.UpdateVolunteerStatus(min, hours, phone, avilabilityStatus, address, longitude, latitude);

        }

        public void UpdateStatus(int min, int hours, string phone, bool avilabilityStatus)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            dbs.UpdateVolunteerStatus(min, hours, phone, avilabilityStatus);

        }


        public void ResetVolunteerStatus(int min, int hours, string phone, bool avilabilityStatus, string address, double longitude, double latitude)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            dbs.ResetVolunteerStatus(min, hours, phone, avilabilityStatus, address, longitude, latitude);

        }

        public object GetVolunteerRelevantRequest(string VolunteerPhone)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.GetVolunteerRelevantRequest(VolunteerPhone);
        }

        public object GetVolunteerHandleRequest(string VolunteerPhone)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.GetVolunteerHandleRequest(VolunteerPhone);
        }

        public int declineRequest(string VolunteerPhone, int RequestId)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.declineRequest(VolunteerPhone, RequestId);

        }

        public int AssociateRequestToVolunteer(string VolunteerPhone, int RequestId)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.AssociateRequestToVolunteer(VolunteerPhone, RequestId);

        }
        

        public int CancelRequestFromVolunteer(int RequestId)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.CancelRequestFromVolunteer(RequestId);

        }

        public int HandelingRequestsVolunteer(string VolunteerPhone)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.HandelingRequestsVolunteer(VolunteerPhone);

        }

        public int updateVolunteerPressTime(DateTime pressTime, string phone)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.updateVolunteerPressTime(pressTime, phone);

        }

        public object GetPushTokensForChat(int expertiseId,string phone)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.GetPushTokensForChat(expertiseId,phone);

        }
        public List<string> GetAllVolunteers()
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.GetAllVolunteers();
        }

        public int UpdateLocation(string VolunteerPhone, string address, double longitude, double latitude)
        {
            DBservicesVolunteer dbs = new DBservicesVolunteer();
            return dbs.UpdateLocation(VolunteerPhone,address,longitude,latitude);

        }
    }
}
