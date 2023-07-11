using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Resc4you_Backend.Models
{
    public class Worker : Person
    {
        string workerType;

        public string WorkerType { get => workerType; set => workerType = value; }
        public ActionResult loginWorker(string phone, string password)
        {
            DBservicesWorker dbs = new DBservicesWorker();
            Worker worker = new Worker();
            worker = dbs.loginWorker(phone);

            if (worker.Phone == null)//worker not exists in db
                return new NotFoundObjectResult(JsonConvert.SerializeObject(new { message = "Incorrect phone or password." }));

            //in case worker exist in db
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, worker.Password);//check the password

            if (!isPasswordValid)//password wrong
            {
                return new OkObjectResult(JsonConvert.SerializeObject(new { message = "Incorrect phone or password." }));
            }
            //password correct
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("dasdsahkhkadsdasdsaads");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.Name, worker.Phone),
                     new Claim(ClaimTypes.Email, worker.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);//create token for the worker

            var response = new { Token = tokenHandler.WriteToken(token), Phone = worker.Phone, FName = worker.FName, LName = worker.LName, Email = worker.Email, WorkerType = worker.workerType,IsActive = worker.IsActive };
            return new OkObjectResult(JsonConvert.SerializeObject(response));

        }
        public int Insert()
        {
            DBservicesWorker dbs = new DBservicesWorker();
            this.Password = BCrypt.Net.BCrypt.HashPassword(this.Password);
            dbs.InsertPerson(this);
            return dbs.InsertWorker(this);

        }


        public object GetWorkersDetails()
        {
            DBservicesWorker dbs = new DBservicesWorker();
            return dbs.GetWorkersDetails();

        }

        public int Update()
        {
            DBservicesWorker dbs = new DBservicesWorker();
            this.Password = BCrypt.Net.BCrypt.HashPassword(this.Password);
            return dbs.UpdatePerson(this);

        }
    }
}
