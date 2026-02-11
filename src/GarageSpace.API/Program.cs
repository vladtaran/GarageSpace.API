using GarageSpace;
using GarageSpace.API.Mapping;
using GarageSpace.Common;
using GarageSpace.Data.Models.MongoDB;
using GarageSpace.EventBus.SDK.Extensions;
using GarageSpace.Repository.EntityFramework;
using GarageSpace.Repository.Interfaces.EF;
using GarageSpace.Repository.Interfaces.MongoDB;
using GarageSpace.Repository.MongoDB;
using GarageSpace.Repository.MongoDB.DbContext;
using GarageSpace.Services;
using GarageSpace.Services.Interfaces;
using GarageSpaceAPI.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var useMongoDB = builder.Configuration.GetValue<bool>("UseMongoDB");
if (useMongoDB)
{
    UseMongoDB(builder);
}
else
{
    UseMsSQL(builder);
}

builder.Services.AddMassTransitEventBus(builder.Configuration);

builder.Services.AddTransient<IPasswordHasher<GarageSpace.Services.Models.User>, PasswordHasher<GarageSpace.Services.Models.User>>();
builder.Services.AddTransient<IIdGenerator, CustomIdGenerator>();


builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUserObjectsMapper, UserObjectsMapper>();
builder.Services.AddScoped<ICarsService, CarsService>();
builder.Services.AddScoped<IGarageService, GarageService>();
builder.Services.AddScoped<IJournalsService, JournalsService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Garage API", Version = "v1" });
});

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

var origins = appSettings?.AllowedCORSOrignis;
if (origins != null) 
{
    builder.Services.AddCors(setup =>
    {
        setup.AddPolicy("policy",
            config =>
            {
                config
                    .WithOrigins(origins)
                    .WithHeaders("Origin", "X-Requested-Width", "Content-Type", "Accept", "Authorization")
                    .WithMethods("GET", "POST", "PUT", "DELETE");
            });
    });

}

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MainDbContext>();

    context.Database.Migrate();

    MockData.SetupMockData(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarGarage API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("policy");

app.Run();

static void UseMongoDB(WebApplicationBuilder builder)
{
    builder.Services.Configure<Settings>(builder.Configuration.GetSection("MongoDB"));
    builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();

    builder.Services.AddScoped<IMongoDbCarsRepository, GarageSpace.Repository.MongoDB.CarsRepository>();
    builder.Services.AddScoped<IMongoDbUserRepository, GarageSpace.Repository.MongoDB.UserRepository>();
}

static void UseMsSQL(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<MainDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IEFCarsRepository, GarageSpace.Repository.EntityFramework.CarsRepository>();
    builder.Services.AddScoped<IEFGaragesRepository, GaragesRepository>();
    builder.Services.AddScoped<IEFJournalsRepository, JournalsRepository>();
    builder.Services.AddScoped<IEFUserRepository, GarageSpace.Repository.EntityFramework.UserRepository>();
    
}

static void AddAuthentication(WebApplicationBuilder builder, AppSettings appSettings)
{
    var key = Encoding.ASCII.GetBytes(appSettings.JWTSecretKey);
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}

