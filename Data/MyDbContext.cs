using Microsoft.EntityFrameworkCore;
using ASP.NET_CA.Models;

namespace ASP.NET_CA.Data
{
	public class MyDbContext : DbContext
	{
		public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
		{
		}

		
		public DbSet<Order> Orders { get; set; }	
		public DbSet<Item> Items { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<User> Users { get; set; }

		public DbSet<Activationcode> Activationcodes { get; set; }
	}

	public class DbData
	{
        // chen's servername = LAPTOP-G1FB3B65\SQLEXPRESS
        public static string dbAddress = "SA57";
		public static string serverName = "LAPTOP-G1FB3B65\\SQLEXPRESS";
		public static string CONNECTION_STRING = @"Server="+serverName+";Database="+dbAddress+";Integrated Security=true;encrypt=false";
	}
}

