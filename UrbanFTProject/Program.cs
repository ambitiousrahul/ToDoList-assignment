using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using UrbanFTProject.Data;
using UrbanFTProject.ToDoList.Data;
using UrbanFTProject.ToDoList.Web.Middlewares;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddScoped<ValidateActionParametersAttribute>();
//builder.Services.AddScoped<CustomUnAuthorizedFilter>();

//to insure urls are always in lowercase.
builder.Services.Configure<RouteOptions>(configureoptions =>
{
    configureoptions.LowercaseUrls = true;
    configureoptions.LowercaseQueryStrings = true;    
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.Configure<FormOptions>(configureoptions =>
{
    configureoptions.ValueCountLimit = 5000;
});



builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDBServices(builder.Configuration);

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddJWTConfigurations(builder.Configuration);

builder.Services.AddControllersWithViews();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


//#region SwaggerService
builder.Services.AddSwaggerGen(options =>
{

    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoList_api", Version = "1.0" });
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Just paste the created JWT token in textbox: \"{Value}\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseGlobalHandlerException();
}
else
{
    app.UseGlobalHandlerException();

    app.UseExceptionHandler("/Home/Error");
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// a middleware to modify the response of any UnAuthorized Responses
app.UseCustomUnauthorizedMiddleware();

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

/*Not Needed*/
//app.MapControllerRoute(
//    name: "api",
//    pattern: "api/{controller=Accounts}/{action=Login}");

app.MapRazorPages();

// Map Web API routes
app.MapControllers();


app.Run();
