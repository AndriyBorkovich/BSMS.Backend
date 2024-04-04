using System.Text;
using FluentValidation.Results;

namespace BSMS.Application.Extensions;

public static class ValidationFailureExtensions
{
    public static string ToResponse(this IEnumerable<ValidationFailure> errorsList)
    {
        return string.Join(" ", errorsList.Select(e => e.ErrorMessage));
    }
}