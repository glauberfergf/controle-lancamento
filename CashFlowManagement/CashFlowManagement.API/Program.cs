using CashFlowManagement.CrossCutting;
using CashFlowManagement.Domain.Configuration;
using CashFlowManagement.Domain.RabbitMq;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region Configs

//Converting enums values
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

//AutoMapper Config
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Db Config
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));

//RabbitMQ Config
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfig"));

#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DI with CrosCurtting Layer
builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CashFlowManagement API V1");
        c.RoutePrefix = "swagger";
    });
}


app.MapControllers();

app.Run();
