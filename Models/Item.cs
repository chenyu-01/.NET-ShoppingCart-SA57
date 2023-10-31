using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET_CA.Models
{
    public class Item
    {
        [Key]
        public int ItemID { get; set; }
        
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public string ItemDescription { get; set; }
        public string ItemImage { get; set; }

    }
}