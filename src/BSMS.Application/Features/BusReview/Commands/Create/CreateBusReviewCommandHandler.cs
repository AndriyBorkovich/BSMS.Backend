using System.Net;
using BSMS.Application.Contracts.Persistence;
using BSMS.Application.Extensions;
using BSMS.Application.Helpers;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace BSMS.Application.Features.BusReview.Commands.Create;

public class CreateBusReviewCommandHandler(
        IBusReviewRepository repository,
        IMapper mapper,
        IValidator<CreateBusReviewCommand> validator,
        MethodResultFactory methodResultFactory)
    : IRequestHandler<CreateBusReviewCommand, MethodResult<int>>
{
    public async Task<MethodResult<int>> Handle(CreateBusReviewCommand request, CancellationToken cancellationToken)
    {
        var result = methodResultFactory.Create<int>();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            result.SetError(validationResult.Errors.ToResponse(), HttpStatusCode.BadRequest);
        }

        var review = mapper.Map<Core.Entities.BusReview>(request);

        await repository.InsertAsync(review);

        result.Data = review.BusReviewId;

        return result;
    }
}