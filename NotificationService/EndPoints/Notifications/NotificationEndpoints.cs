using NotificationService.Models;
using NotificationService.Services.Email;
using NotificationService.Services.SMS;

namespace NotificationService.EndPoints.Notifications;

public static class NotificationEndpoints
{
    public static IEndpointRouteBuilder MapNotificationEndpoints(this WebApplication app)
    {
        var grp = app.MapGroup("/notifications");
        grp.MapPost("/sms", SendSms);
        grp.MapPost("/email", SendEmail);
        return grp;
    }

    private static async Task<IResult> SendEmail(EmailRequest request, IEmailSender sender)
    {
        var result = await sender.Send(request.Email, request.Subject, request.Message);

        return result.IsSuccess
            ? Results.Ok(new { messageId = result.Value })
            : Results.BadRequest(new { 
                errorCode = result.Error.Code, 
                errorMessage = result.Error.Message 
            });
    }

    private static async Task<IResult> SendSms(SmsRequest request, ISmsSender sender)
    {
        var result = await sender.Send(request.PhoneNumber, request.Message);

        return result.IsSuccess
            ? Results.Ok(new { messageId = result.Value })
            : Results.BadRequest(new { 
                errorCode = result.Error.Code, 
                errorMessage = result.Error.Message 
            });
    }
}