using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Serilog;
using UI.Data;
using UI.Security;
using UI.Services;
using UI.Services.IService;
using UI.Services.MockService;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});
//var log = new LoggerConfiguration()
//    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
//    .CreateLogger();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

//// NLog: Setup NLog for Dependency injection
//builder.Logging.ClearProviders();
//builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
//builder.Host.UseNLog();



builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostGreSQLConnection"));
});


builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleRepository, MockRoleRepository>();
builder.Services.AddScoped<IListItemCategoryRepository, MockListItemCategoryRepository>();
builder.Services.AddScoped<IListItemRepository, MockListItemRepository>();
builder.Services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();
builder.Services.AddScoped<IPersonRepository, MockPersonRepository>();
builder.Services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();
//builder.Services.Configure<MailSettings>(("MailSettings"));
builder.Services.AddSingleton<DataProtectionPurposeStrings>();
builder.Configuration.GetSection("MailSettings");
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<IDashboardService, MockDashboard>();

//For cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
    });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");

}

app.UseHttpsRedirection();
app.UseStaticFiles();

//for cookie authentication and authorization
app.UseAuthentication();


app.MapRazorPages();
app.MapDefaultControllerRoute();

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
