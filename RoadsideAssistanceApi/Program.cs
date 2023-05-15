using RoadsideAssistanceApi.ActionProcessor;
using RoadsideAssistanceApi.Repository;
using RoadsideAssistanceBL.DataStore;
using RoadsideAssistanceBL.Service;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/roadservice.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRoadSideDataStore, RoadSideDataStore>();
builder.Services.AddSingleton<IAssistantService, AssistantService>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRoadsideAssistanceRepository, RoadsideAssistanceRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAssistantRepository, AssistantRepository>();
builder.Services.AddScoped<IAssistantService, AssistantService>();
builder.Services.AddScoped<IRoadsideAssistanceActionProcessor, RoadsideAssistanceActionProcessor>();
builder.Services.AddScoped<IRoadsideAssistanceService, RoadsideAssistanceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }