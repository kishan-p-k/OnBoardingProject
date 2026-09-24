using BT.Implementation.Providers.ADO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BT.Implementation.Providers;
using BT.Implementation;
using BT;
using Serilog;



var builder = WebApplication.CreateBuilder(args);


////builder.Configuration["Jwt:Key"];
//// Add services to the container.
builder.Services.AddControllers();

// Add OpenAPI/Swagger support
builder.Services.AddEndpointsApiExplorer(); // ADDED
builder.Services.AddSwaggerGen();            // ADDED

// Register database
builder.Services.AddSingleton<DatabaseConnection>();

//Add DependencyInjection
builder.Services.AddImplementation();

// Add Serilog configuration
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));



// Add CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//builder.Services.AddAuthentication(
//    JwtBearerDefaults.AuthenticationScheme
//)
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,

//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(
//                builder.Configuration["Jwt:Key"]!
//            )
//        ),

//        ValidateIssuer = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],

//        ValidateAudience = true,
//        ValidAudience = builder.Configuration["Jwt:Audience"],

//        ValidateLifetime = true,

//        ClockSkew = TimeSpan.Zero
//    };
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
