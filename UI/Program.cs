using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using UI.Data;
using UI.Security;
using UI.Services;
using UI.Services.IService;
using UI.Services.MockService;
using UI.ViewModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

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
    app.UseExceptionHandler("/Error /Error");
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
