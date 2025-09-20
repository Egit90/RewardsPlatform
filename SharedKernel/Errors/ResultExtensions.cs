using CSharpFunctionalExtensions;

namespace SharedKernel.Errors;

public static class ResultExtensions
{
    public static Result<T> Failure<T>(this Result<T> result, Error error) =>
        Result.Failure<T>($"{error.Code}: {error.Message}");
 
    public static Result<T> ToFailure<T>(this Result result, Error error) =>
        result.IsFailure
            ? Result.Failure<T>($"{error.Code}: {error.Message}")
            : Result.Failure<T>("Unexpected success state.");
}