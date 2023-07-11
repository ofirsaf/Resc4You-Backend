using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Net;
using System.Numerics;

namespace Resc4you_Backend.Models
{
    public class Request
    {
        int requestId;
        string requestAddress;
        double requestLongitude;
        double requestLatitude;
        string licenseNum;
        string requestStatus;
        DateTime requestDate;
        string requestSummary;
        string citizenPhone;
        string workerPhone;
        int manufacturerId;
        int specialtyId;

        public int RequestId { get => requestId; set => requestId = value; }
        public string RequestAddress { get => requestAddress; set => requestAddress = value; }
        public double RequestLongitude { get => requestLongitude; set => requestLongitude = value; }
        public double RequestLatitude { get => requestLatitude; set => requestLatitude = value; }
        public string LicenseNum { get => licenseNum; set => licenseNum = value; }
        public string RequestStatus { get => requestStatus; set => requestStatus = value; }
        public DateTime RequestDate { get => requestDate; set => requestDate = value; }
        public string RequestSummary { get => requestSummary; set => requestSummary = value; }
        public string CitizenPhone { get => citizenPhone; set => citizenPhone = value; }
        public string WorkerPhone { get => workerPhone; set => workerPhone = value; }
        public int ManufacturerId { get => manufacturerId; set => manufacturerId = value; }
        public int SpecialtyId { get => specialtyId; set => specialtyId = value; }

        public int Insert()
        {
            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.InsertRequest(this);
        }
        public object getAllRequest()
        {
            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.GetAllRequests();

        }
        public object GetRelevantVolunteer(int specialtyId)
        {
            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.GetRelevantVolunteer(specialtyId);

        }

        public object GetRelevantRequestsToVolunteer(string VolunteerPhone)
        {
            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.GetRelevantRequestsToVolunteer(VolunteerPhone);

        }

        
        public int InsertToVolunteerOfRequest(string phone,int requestId)
        {
            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.InsertToVolunteerOfRequest(phone, requestId);
        }

        public string GetVolunteerHandlePushTokken(int requestId)
        {
            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.GetVolunteerHandlePushTokken(requestId);
        }

        public int RequestAlreadyTaken(int requestId)
        {
            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.RequestAlreadyTaken(requestId);
        }

        public int UpdateRateOfRequest(int requestId, string review, int rating)
        {
            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.UpdateRateOfRequest(requestId, review, rating);
        }

        public object GetRequestAndVolunteerLocation(int requestId, string phone)
        {
            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.GetRequestAndVolunteerLocation(requestId, phone);
        }
        public int UpdateRequestStatusToClose(int requestId, string phone,string address, double longi, double lati)
        {
            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.UpdateRequestStatusToClose(requestId,phone,address,longi,lati);
        }


        public object GetStatusOfSpecificRequest(int requestId)
        {

            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.GetStatusOfSpecificRequest(requestId);

        }

        public object GetDetailsOfCitizenOpenedRequest(int requestId)
        {

            DBservicesRequest dbs = new DBservicesRequest();
            return dbs.GetDetailsOfCitizenOpenedRequest(requestId);

        }

    }
}
