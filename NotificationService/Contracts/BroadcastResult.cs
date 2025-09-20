using CSharpFunctionalExtensions;
using SharedKernel.Errors;

namespace NotificationService.Contracts;

public sealed record BroadcastResult(string Channel, string Recipient, string Status, string? Error)
{
    public static BroadcastResult FromResult(Result<string, Error> res, Channel channel, string recipient)
        => new(channel.ToString(),
            recipient,
            res.IsSuccess ? "sent" : "failed",
            res.IsFailure ? res.Error.Message : null);
};