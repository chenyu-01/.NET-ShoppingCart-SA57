using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_CA.Models
{
    public class Order
    {
        public Guid OrderID { get; set; } // set as primary key
		public string OrderStatus { get; set; }
        public string OrderDate { get; set; }



        // foreign key from User
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        // foreign key from Item
        [ForeignKey("ItemID")]
        public virtual Item Item { get; set; }
        // can have multiple activation code when purchase
        public int Quantity { get; set; }

        // one to many relationship with Activationcode
        [ForeignKey("OrderID")]
        public virtual ICollection<Activationcode> Activationcodes { get; set; }


    }
}