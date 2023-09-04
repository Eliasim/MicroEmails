using ApiARI.Interfaces;
using ApiARI.Model;
using ApiARI.Services;
using ApiARI.Utilities;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<MailFactory>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapPost("/Email", async (EmailBody emailBody, HttpRequest request, IEmailService email) =>
{
    try
    {
        var mail = await email.SendReceptionCertificate(emailBody);

        if (mail == false) return Results.NotFound();

        return Results.Ok(emailBody);
    }
    catch (Exception e)
    {
        if (e.GetType() == typeof(ValidationException))
            return Results.Problem(e.Message, statusCode: 400);
        return Results.Problem(e.Message);
    }
})
    .WithName("SendEmail")
    .Produces<IResult>()
    .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
    .Produces<HttpValidationProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")
    .AllowAnonymous();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
