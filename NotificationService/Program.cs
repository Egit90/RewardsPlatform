using Amazon.SimpleNotificationService;
using NotificationService.Models;
using NotificationService.Services;
using NotificationService.Services.Email;
using NotificationService.Services.SMS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<ISmsSender, FakeSmsSender>();
    builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
}
else
{
    builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
    builder.Services.AddAWSService<IAmazonSimpleNotificationService>();
    builder.Services.AddScoped<ISmsSender, SmsSender>();
    builder.Services.AddScoped<IEmailSender, SesEmailSender>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/notifications/sms",
    async (SmsRequest smsRequest, ISmsSender sender) =>
    {
        var result = await sender.Send(smsRequest.PhoneNumber,smsRequest.Message);
        return result.IsSuccess
            ? Results.Ok(new { messageId = result.Value })
            : Results.BadRequest(new { error = result.Error });
    });

app.Run();

