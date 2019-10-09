using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPICourse
{
    public interface IUserService
    {
       string getName();
    }
    public class User:IUserService
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Token { get; set; }

        public string getName()
        {
            return "Test Ioc Application";
        }

        
    }


}
