using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_CA.Models
{
    public class User
    {
        public User()
        {
            UserID = Guid.NewGuid();
        }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        
    }
}