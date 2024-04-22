using System.ComponentModel.DataAnnotations;

namespace BSMS.Application.Features.Common;

public class ListResponse<TResult>
{
    public ListResponse(List<TResult> result, int total)
    {
        Result = result;
        Total = total;
    }

    [Required]
    public List<TResult> Result { get; set; }
    [Required]
    public int Total { get; set; }
}