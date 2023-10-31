using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_CA.Models
{
    

    public class Activationcode
    {
        public Guid OrderID { get; set; }
        [Key]
        public string Value { get; set; }
        public Activationcode()
        {
            Value = "";
            Random random = new Random();
            // generate a random string for the activation code xxx-xxx-xxx-x
            for (int i = 0; i < 3; i++)
            {
                int x = random.Next(1, 5); //random length of the substring
                Value += System.Guid.NewGuid().ToString().Substring(0, x) + "-";
            }
            Value += System.Guid.NewGuid().ToString().Substring(1, 4);
            
        }
    }
}
