using IdentityService.Contracts;
using SharedKernel.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapPost("/identity/signin", (SignInRequest request) =>
{
    var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
    if (phoneNumber.IsFailure)
    {
        return Results.BadRequest(new { error = phoneNumber.Error });
    }
    
    // todo: send OTP via Notification service
    var sessionId = Guid.NewGuid().ToString();
    
    return Results.Ok(new { sessionId = sessionId });
})
.WithName("SignIn");

app.Run();

