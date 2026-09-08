using FluentValidation;
using ProcurementSys.Application.Features.ProcurementRequests;

namespace ProcurementSys.Application.Features.ProcurementRequests.Validators;

public class AssignVendorCommandValidator : AbstractValidator<AssignVendorCommand>
{
    public AssignVendorCommandValidator()
    {
        RuleFor(x => x.ProcurementRequestId)
            .GreaterThan(0)
            .WithMessage("Procurement Request ID tidak valid.");

        RuleFor(x => x.VendorId)
            .GreaterThan(0)
            .WithMessage("Vendor ID tidak valid.");
    }
}