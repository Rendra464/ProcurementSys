using FluentValidation;

namespace ProcurementSys.Application.Features.MaterialRequests.Validators;

public class SubmitMaterialRequestCommandValidator : AbstractValidator<SubmitMaterialRequestCommand>
{
    public SubmitMaterialRequestCommandValidator()
    {
        RuleFor(x => x.MaterialRequestId)
            .GreaterThan(0)
            .WithMessage("MaterialRequestId tidak valid.");
    }
}