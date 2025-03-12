using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using VotingApp.API.Data;
using VotingApp.API.Middlewares;
using VotingApp.API.Models;
using VotingApp.API.Services;
using VotingApp.API.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddScoped<IPartyService, PartyService>();
builder.Services.AddScoped<IStateResults, StateResults>();
builder.Services.AddScoped<INationalResult, NationalResult>();


builder.Services.AddScoped<IVoterService, VoterService>();
builder.Services.AddScoped<IVoteService, VoteService>();
builder.Services.AddScoped<IVoteSessionService, VoteSessionService>();

builder.Services.AddScoped<ICandidateService, CandidateService>();




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<VotingAppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("VotingAppConnectionString")
    ));




builder.Services.AddIdentity<AuthUser, IdentityRole>()
    .AddEntityFrameworkStores<VotingAppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your JWT token}' below.\n\nExample: 'Bearer eyJhbGciOiJIUzI1NiIs...'"
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





var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"]
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            
            context.HandleResponse(); 
            var errorResponse = new
            {
                error = true,
                code = 401,
                errorMessage = "User not authorized"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    };
});



builder.Services.AddAuthorization();




var app = builder.Build();

           

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
