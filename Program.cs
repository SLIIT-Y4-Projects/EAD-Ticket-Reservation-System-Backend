using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using TicketReservationSystemAPI.Identity;
using TicketReservationSystemAPI.Models;
using TicketReservationSystemAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// JWT
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityData.BackOfficePolicyName, policy => policy.RequireClaim(IdentityData.BackOfficeClaimName, "true"));
    options.AddPolicy(IdentityData.TravelAgentPolicyName, policy => policy.RequireClaim(IdentityData.TravelAgentClaimName, "true"));
    options.AddPolicy(IdentityData.TravellerPolicyName, policy => policy.RequireClaim(IdentityData.TravellerClaimName, "true"));
});

// Add services to the container.
builder.Services.Configure<TicketReservationDatabaseSettings>(
                builder.Configuration.GetSection(nameof(TicketReservationDatabaseSettings)));
builder.Services.AddSingleton<ITicketReservationDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<TicketReservationDatabaseSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("TicketReservationDatabaseSettings:ConnectionString")));
builder.Services.AddScoped <IExampleService, ExampleService>();
builder.Services.AddScoped <IBackOfficeUserService, BackOfficeUserService>();
builder.Services.AddScoped <ITravelAgentUserService, TravelAgentUserService>();
builder.Services.AddScoped <ITravellerUserService, TravellerUserService>();
builder.Services.AddScoped <ITrainService, TrainService>();
builder.Services.AddScoped <IReservationService, ReservationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
