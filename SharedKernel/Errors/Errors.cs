namespace SharedKernel.Errors;

public static class MainErrors
{
    public static readonly Error InvalidPhone =
        Error.Validation("phone.invalid", "Phone number is invalid.");

    public static readonly Error CustomerNotFound =
        Error.NotFound("customer.not_found", "Customer was not found.");
}