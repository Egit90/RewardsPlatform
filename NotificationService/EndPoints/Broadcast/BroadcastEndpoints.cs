using NotificationService.Contracts;
using NotificationService.Services.Email;
using NotificationService.Services.SMS;

namespace NotificationService.EndPoints.Broadcast;

public static class BroadcastEndpoints
{
    public static IEndpointRouteBuilder MapBroadcast(this WebApplication app)
    {
        var group = app.MapGroup("/notifications");
        group.MapPost("/broadcast", BroadcastMessage);
        return group;
    }

    private static async Task<IResult> BroadcastMessage(BroadcastRequest req, ISmsSender sms, IEmailSender email)
    {
        var results = new List<BroadcastResult>();

        foreach (var recipient in req.Recipients ?? Enumerable.Empty<string>())
        {
            foreach (var channel in req.Channels ?? Enumerable.Empty<Channel>())
            {
                switch (channel)
                {
                    case Channel.Email:
                    {
                        var r = await email.Send(recipient, req.Subject, req.Message);
                        results.Add(BroadcastResult.FromResult(r, channel, recipient));
                        break;
                    }

                    case Channel.Sms:
                    {
                        var r = await sms.Send(recipient, req.Message);
                        results.Add(BroadcastResult.FromResult(r, channel, recipient));
                        break;
                    }
                }
            }
        }

        return Results.Ok(results);
    }
}