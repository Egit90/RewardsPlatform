using Microsoft.EntityFrameworkCore;
using RewardsService.Data;
using RewardsService.EndPoints.Awards;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<RewardsDbContext>(ops =>
{
    ops.UseNpgsql(builder.Configuration.GetConnectionString("RewardsDb"));
});

var app = builder.Build();
app.MapAwards();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

