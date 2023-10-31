using ASP.NET_CA.Data;
using ASP.NET_CA.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
// Add database context
builder.Services.AddDbContext<MyDbContext>(options =>
{
    var conn_str = builder.Configuration.GetConnectionString("conn_str");
    options.UseLazyLoadingProxies().UseSqlServer(conn_str);

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller}/{action}/{id?}");

// clean database
InitDB(app.Services);
// add a user and some items to the database
getMockData(app.Services);


app.MapControllerRoute(
	name: "Login",
	pattern: "{controller=Login}/{action=Login}");

app.MapControllerRoute(
	name: "Gallery",
	pattern: "{controller=Gallery}/{action=Gallery}");

app.MapControllerRoute(
	name: "MyPurchase",
	pattern: "{controller=MyPurchase}/{action=MyPurchase}");

app.MapControllerRoute(
	name: "ViewCart",
	pattern: "{controller=ViewCart}/{action=ViewCart}");
app.Run();

void InitDB(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    MyDbContext db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    //if (!db.Database.CanConnect())
    {
	    db.Database.EnsureDeleted();
	    db.Database.EnsureCreated();
	}
}

// Mock Data to be used for testing
void getMockData(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    MyDbContext _db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    // Mock User
    User user = new User { UserID = Guid.NewGuid(), UserName = "chen", UserPassword = "yu" };
    // Temp user that not logged in
    User unknown = new User { UserID = Guid.NewGuid(), UserName = "unknown", UserPassword = "unknown" };
    _db.Users.Add(user);
    _db.Users.Add(unknown);
    String[,] test_data = new String[,]
    {
                {
                    ".NET Charts", "99", "Brings powerful charting capabilities to your .NET application",
                    "dotnet_charts.png"
                },
                { ".NET Paypal", "69", "Integrate your .NET apps with your paypal the easy way", "paypal.png" },
                { ".NET ML", "299", "Supercharged .NET machine learning libraries", "mlnet.png" },
                { ".NET Analytics", "299", "Performs data mining and analytics in .NET", "analytics.png" },
                { ".NET Logger", "49", "Logs and aggregates events easily in your .NET application", "lognet.png" },
                { ".NET Numerics", "199", "Powerful numerical methods for your .NET simulations", "numerics.png" }
    };


    for (int i = 0; i < test_data.GetLength(0); i++)
    {
        Guid actCode = Guid.NewGuid();
        // Mock Item
        Item item = new Item
        {
            ItemName = test_data[i, 0],
            ItemPrice = Double.Parse(test_data[i, 1]),
            ItemDescription = test_data[i, 2],
            ItemImage = test_data[i, 3],
        };
        //items_test.Add(item);
        _db.Items.Add(item);
    }

    _db.SaveChanges();
}