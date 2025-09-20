using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using SharedKernel.Errors;

namespace SharedKernel.Domain;

public class PhoneNumber : ValueObject
{
    public string Value { get; }
    private PhoneNumber(string value) => Value = value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<PhoneNumber, Error> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result.Failure<PhoneNumber, Error>(MainErrors.InvalidPhone);
        }

        var regex = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
        return !regex.IsMatch(value) 
                ? Result.Failure<PhoneNumber, Error>(MainErrors.InvalidPhone) 
                : Result.Success<PhoneNumber, Error>(new PhoneNumber(value));
    }
}