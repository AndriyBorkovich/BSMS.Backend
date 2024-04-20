using FluentValidation.Results;

namespace BSMS.Application.Extensions;

public static class ValidationFailureExtensions
{
    public static string ToResponse(this IEnumerable<ValidationFailure> errorsList)
    {
        return errorsList.First().ErrorMessage;
    }
}