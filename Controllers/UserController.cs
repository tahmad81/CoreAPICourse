using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CoreAPICourse.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        JWTSettings _settings;
        private User _user;
        public UsersController(IOptions<JWTSettings> settings,IUserService user)
        {
            _settings = settings.Value;
            _user = user as User; 
        }
        
        [HttpPost("login")]
        [EnableCors("AllowOrigin")]
        public ActionResult<User> Login(User model)
        {
            model.Token = createToken(model);
            return model;
       }
        [HttpGet("userlist")]
        [Authorize]
        [EnableCors("AllowOrigin")]
        public ActionResult<List<User>> GetList()
        {
             return new List<User>() { new User { Id = 1, Username ="user1" ,Firstname="Firstname1", Lastname= "Last Name1" }, new User { Id = 2, Username="user2", Firstname="Firstname2", Lastname= "Last Name2" },
             new User{ Username = "user3",Firstname="Firstname3", Lastname= "Last Name3" }, new User{ Username="user4",Firstname="Firstname4", Lastname= "Last Name4" }

            };

        }
        [HttpPost("register")]
        public ActionResult<string> Register(User model)
        {
            return createToken(model);
        }

        // GET api/values
        [HttpGet("gettoken")]
        public ActionResult<string> Get()
        {
            throw new Exception("Test Exception");
          //  return createToken(new User { Firstname = "test", Lastname = "test", Id = 11, Username = "test" });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("protected")]
        public ActionResult<string> GetProtectedMessage()
        {
            return "Message from Protected route";

        }
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        string createToken(User model)
        {
            var key = Encoding.ASCII.GetBytes(_settings.SecretKey);
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokendescriptor = new SecurityTokenDescriptor

            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,model.Firstname + " " + model.Lastname)
                }),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenhandler.CreateToken(tokendescriptor);
            return tokenhandler.WriteToken(token);

        }


    }

}
