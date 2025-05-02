using FluentValidation;
using FluentValidation.AspNetCore;
using GoodHamburger.API.Configurations;
using GoodHamburger.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("GoodHamburgerDb")
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.ResolveDependencies();
builder.Services.AddFluentValidationAutoValidation(); 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();
    await DatabaseSeeder.SeedAsync(dbContext);
}

app.Run();
