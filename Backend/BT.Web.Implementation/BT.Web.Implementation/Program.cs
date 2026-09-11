using BT.Implementation.Providers.ADO;
using BT.Implementation.Providers.Interfaces;
using BT.Implementation.Services;
using BT.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add OpenAPI/Swagger support
builder.Services.AddOpenApi();

// Register database
builder.Services.AddSingleton<DatabaseConnection>();

// Register providers
builder.Services.AddScoped<IBugProvider, BugProvider>();
builder.Services.AddScoped<IAuthProvider, AuthProvider>();

// Register services
builder.Services.AddScoped<IBugService, BugService>();
builder.Services.AddScoped<IAuthService, AuthService>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
