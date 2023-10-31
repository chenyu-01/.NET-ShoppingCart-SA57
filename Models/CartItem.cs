using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_CA.Models
{
    
    public class CartItem 
    {
        [Key]
        public int CartItemID { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ItemID")]
        public virtual Item Item { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}
