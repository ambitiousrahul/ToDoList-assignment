using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using UrbanFTProject.Data;
using UrbanFTProject.ToDoList.Data;
using UrbanFTProject.ToDoList.Web.Middlewares;
using Microsoft.AspNetCore.Http.Features;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddScoped<ValidateActionParametersAttribute>();

//to insure urls are always in lowercase.
builder.Services.Configure<RouteOptions>(configureoptions =>
{
    configureoptions.LowercaseUrls = true;
});

builder.Services.Configure<FormOptions>(configureoptions =>
{
    configureoptions.ValueCountLimit = 5000;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;
    options.LoginPath = new PathString("/accounts/login");
    options.ReturnUrlParameter = "returnUrl";
});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDBServices(builder.Configuration);

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddJWTConfigurations(builder.Configuration);

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//          .AddCookie(options =>
//          {
//              options.LoginPath = new PathString("/account/login");
//              options.ReturnUrlParameter = "returnUrl";
//          });

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
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
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

// Map Web API routes
app.MapControllers();


app.Run();
