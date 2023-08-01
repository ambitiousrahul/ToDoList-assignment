using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using UrbanFTProject.Data;
using UrbanFTProject.ToDoList.Data;
using UrbanFTProject.ToDoList.Web.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

//to insure urls are always in lowercase.
builder.Services.Configure<RouteOptions>(configureoptions =>
{
    configureoptions.LowercaseUrls = true;
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDBServices(builder.Configuration);

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();


//#region SwaggerService
builder.Services.AddSwaggerGen(options =>
{

    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoList_api", Version = "1.0" });
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseGlobalHandlerException();

    app.UseExceptionHandler("/Home/Error");
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("swagger/v1/swagger.json", "ToDoList_api");

    // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
    c.RoutePrefix = string.Empty;
    c.DefaultModelsExpandDepth(-1);
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
