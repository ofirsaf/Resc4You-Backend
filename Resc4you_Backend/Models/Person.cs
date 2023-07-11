using BCrypt.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Resc4you_Backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Resc4you_Backend.Models
{
    public class Person
    {
        string phone;
        string fName;
        string lName;
        string password;
        string email;
        string personType;
        string expo_push_token;
        bool isActive;

        public string Phone { get => phone; set => phone = value; }
        public string FName { get => fName; set => fName = value; }
        public string LName { get => lName; set => lName = value; }
        public string Password { get => password; set => password = value; }
        public string Email { get => email; set => email = value; }
        public string PersonType { get => personType; set => personType = value; }
        public string Expo_push_token { get => expo_push_token; set => expo_push_token = value; }
        public bool IsActive { get => isActive; set => isActive = value; }

        public ActionResult loginUser(string phone, string password)
        {
            DBservicesPerson dbs = new DBservicesPerson();
            Person person = new Person();
            person = dbs.loginUser(phone);

            if (person.Phone == null)//user not exist in db
                return new NotFoundObjectResult(JsonConvert.SerializeObject(new { message = "Incorrect phone or password." }));
            //in case user exist in db
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, person.Password);//check the password
            if (!isPasswordValid)//in case password inccorrect
            {
                return new OkObjectResult(JsonConvert.SerializeObject(new { message = "Incorrect phone or password." }));
            }
            //in case password correct
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("dasdsahkhkadsdasdsaads");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.Name, person.Phone),
                     new Claim(ClaimTypes.Email, person.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);//create token for the user

            var response = new { Token = tokenHandler.WriteToken(token), Phone = person.Phone, FName = person.FName, LName = person.LName, Email = person.Email,PersonType=person.PersonType,IsActive=person.IsActive };
            return new OkObjectResult(JsonConvert.SerializeObject(response));

        }

        public int UpdateUserPushNotificationToken(string phone, string pushNotificationToken)
        {
            DBservicesPerson dbs = new DBservicesPerson();
            return dbs.UpdateUserPushNotificationToken(phone, pushNotificationToken);
        }

        public object GetUserDetails(string phone)
        {
            DBservicesPerson dbs = new DBservicesPerson();
            return dbs.GetUserDetails(phone);
        }

        public int UpdateIsActive(string phone, bool isActive)
        {
            DBservicesPerson dbs = new DBservicesPerson();
            return dbs.UpdateIsActive(phone,isActive);
        }
    }
}
