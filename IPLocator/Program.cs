using IPLocator.Data;
using IPLocator.Data.Repository.Abstracts;
using IPLocator.Data.Repository.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString : builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

/////------------- #START# AUTO REPOSITORY ADD DI CONTAINER ---------------------////
var allProviderTypes = System.Reflection.Assembly.GetExecutingAssembly()
  .GetTypes().Where(t => t.Namespace != null && t.Namespace.Contains("Repository")); 

foreach (var intfc in allProviderTypes.Where(t => t.IsInterface))
{
    var impl = allProviderTypes.FirstOrDefault(c => c.IsClass && intfc.Name.Substring(1) == c.Name);
    if (impl != null) builder.Services.AddScoped(intfc, impl);
}
/////------------- #END# AUTO REPOSITORY ADD DI CONTAINER ---------------------////


builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost()
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
