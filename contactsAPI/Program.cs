using contactsAPI.Data;
using contactsAPI.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
// using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<ContactsAPIDbContext>(options => options.UseInMemoryDatabase("ContactsDB"));

builder.Services.AddDbContext<ContactsAPIDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("ContactsApiConnectionString")));

builder.Services.AddScoped<IContactService, ContactService>();

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
