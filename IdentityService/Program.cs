using IdentityService.Contracts;
using IdentityService.Models;
using IdentityService.Services;
using SharedKernel.Domain;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("redis:6379"));
builder.Services.AddScoped<IOtpStore, RedisOtpStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapPost("/identity/signin",
        async (SignInRequest request, IHttpClientFactory httpClientFactory, IOtpStore otpStore) =>
        {
            var phoneResult = PhoneNumber.Create(request.PhoneNumber);
            if (phoneResult.IsFailure)
            {
                return Results.BadRequest(new { error = phoneResult.Error });
            }

            var otp = new Random().Next(100000, 999999).ToString();
            var sessionId = Guid.NewGuid().ToString();

            // save otp
            await otpStore.SaveAsync(sessionId, otp, TimeSpan.FromMinutes(5));

            var client = httpClientFactory.CreateClient("notifications");
            var response = await client.PostAsJsonAsync("/notifications/sms", new
            {
                phoneNumber = phoneResult.Value.Value,
                message = $"Your OTP is {otp}"
            });

            return !response.IsSuccessStatusCode
                ? Results.Problem("Failed to send OTP.")
                : Results.Ok(new SignInResponse("OTP sent to phone", sessionId));
        })
    .WithName("SignIn");


app.MapPost("/identity/verify", async ( VerifyRequest req, IOtpStore otpStore) =>
{
    var stored = await otpStore.GetAsync(req.SessionId);
    if (stored is null)
        return Results.BadRequest(new { error = "Session expired or invalid." });

    if (stored != req.Otp)
        return Results.BadRequest(new { error = "Invalid OTP." });

    // OTP is correct → remove it
    await otpStore.RemoveAsync(req.SessionId);

    return Results.Ok(new { message = "Login successful!" });
});

app.Run();