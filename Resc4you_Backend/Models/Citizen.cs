using BCrypt.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;
using System.Numerics;

namespace Resc4you_Backend.Models
{
    public class Citizen:Person
    {
        public int Insert()
        {
            DBservicesCitizen dbs = new DBservicesCitizen();


            this.Password = BCrypt.Net.BCrypt.HashPassword(this.Password);
            dbs.InsertPerson(this);
            return dbs.InsertCitizen(this);

        }
        public int InsertCitizenLanguage(string CitizenPhone, int CitizenLanguageId)
        {
            DBservicesCitizen dbs = new DBservicesCitizen();
            return dbs.InsertCitizenLanguage(CitizenPhone, CitizenLanguageId);

        }

        public List<Citizen> ReadCitizen(string phone)
        {
            DBservicesCitizen dbs = new DBservicesCitizen();
            return dbs.ReadCitizen(phone);
        }

        public int Update()
        {
            DBservicesCitizen dbs = new DBservicesCitizen();
            this.Password = BCrypt.Net.BCrypt.HashPassword(this.Password);
            return dbs.UpdatePerson(this);

        }
        public object getCitizenRequest(string phone)
        {
            DBservicesCitizen dbs = new DBservicesCitizen();
            return dbs.getCitizenReqests(phone);
        }

        public object GetCitizenOpenRequest(string phone)
        {
            DBservicesCitizen dbs = new DBservicesCitizen();
            return dbs.GetCitizenOpenRequest(phone);
        }

        public int DeleteRequest(int requestId)
        {
            DBservicesCitizen dbs = new DBservicesCitizen();
            return dbs.DeleteRequest(requestId);
        }
        public object GetNumberCitizenOpenRequest(string phone)
        {
            DBservicesCitizen dbs = new DBservicesCitizen();
            return dbs.GetNumberCitizenOpenRequest(phone);
        }
        public object GetDetailsOfReportedRequest(string phone)
        {
            DBservicesCitizen dbs = new DBservicesCitizen();
            return dbs.GetDetailsOfReportedRequest(phone);
        }

    }
}
