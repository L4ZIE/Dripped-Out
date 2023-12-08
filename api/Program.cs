using System.Text;
using infrastructure.DatabaseManager;
using infrastructure.DatabaseManager.Interface;
using infrastructure.Entities;
using infrastructure.Repositories;
using infrastructure.Repositories.Factory;
using infrastructure.Repositories.Interface;
using service.Helpers;
using service.Services;

var builder = WebApplication.CreateBuilder(args);


string secret = "asdfasdfsa";//TODO RETRIEVE SECRET FROM CONFIG FILE OR USE ENV VARIABLE
Byte[] secretBytes = Encoding.ASCII.GetBytes(secret);

builder.Services.AddSingleton<IDBConnection, DBConnection>();
builder.Services.AddSingleton<CRUDFactory>();
builder.Services.AddSingleton<LoginRepository>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<AuthenticationHelper>(new AuthenticationHelper(secretBytes));
builder.Services.AddSingleton<LoginService>();
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.MapControllers();


app.Run();