using Microsoft.EntityFrameworkCore;
using EX8.Models;

var builder = WebApplication.CreateBuilder(args);

// Dodaj usługę DbContext z konfiguracją połączenia do bazy danych
builder.Services.AddDbContext<TripsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodaj usługi do kontenera.
builder.Services.AddControllers();

// Dodaj Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Skonfiguruj potok HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Zmiana portu nasłuchu na 5006
app.Urls.Add("http://localhost:5006");

app.Run();