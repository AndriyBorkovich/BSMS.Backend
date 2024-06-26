﻿using BSMS.Application.Contracts.Persistence;
using FluentValidation;

namespace BSMS.Application.Features.BusReview.Commands.Create;

public class CreateBusReviewCommandValidator : AbstractValidator<CreateBusReviewCommand>
{
    private readonly IBusRepository _busRepository;
    private readonly IPassengerRepository _passengerRepository;
    public CreateBusReviewCommandValidator(IBusRepository busRepository, IPassengerRepository passengerRepository)
    {
        _busRepository = busRepository;
        _passengerRepository = passengerRepository;

        RuleFor(c => c.BusId)
            .MustAsync(async (id, _) => await _busRepository.AnyAsync(b => b.BusId == id))
            .WithMessage("Bus must exist!");
        
        RuleFor(c => c.PassengerId)
            .MustAsync(async (id, _) => await _passengerRepository.AnyAsync(p => p.PassengerId == id))
            .WithMessage("Passenger must exist!");
        
        const int minRating = 1, maxRating = 5;
        
        RuleFor(x => x.ComfortRating)
            .InclusiveBetween(minRating, maxRating)
            .WithMessage("Comfort rating must be between 1 and 5.");

        RuleFor(x => x.PunctualityRating)
            .InclusiveBetween(minRating, maxRating)
            .WithMessage("Punctuality rating must be between 1 and 5.");

        RuleFor(x => x.PriceQualityRatioRating)
            .InclusiveBetween(minRating, maxRating)
            .WithMessage("Price quality ratio rating must be between 1 and 5.");

        RuleFor(x => x.InternetConnectionRating)
            .InclusiveBetween(minRating, maxRating)
            .WithMessage("Internet connection rating must be between 1 and 5.");

        RuleFor(x => x.SanitaryConditionsRating)
            .InclusiveBetween(minRating, maxRating)
            .WithMessage("Sanitary conditions rating must be between 1 and 5.");
    }
}