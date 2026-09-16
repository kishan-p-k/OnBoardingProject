using BT.Implementation.Providers.ADO;
using BT.Implementation.Providers;
using BT.Implementation;
using BT;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
