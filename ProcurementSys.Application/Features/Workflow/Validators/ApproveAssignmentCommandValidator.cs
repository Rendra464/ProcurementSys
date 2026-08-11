using FluentValidation;
using ProcurementSys.Application.Features.Workflow;

namespace ProcurementSys.Application.Features.Workflow.Validators;

public class ApproveAssignmentCommandValidator : AbstractValidator<ApproveAssignmentCommand>
{
    public ApproveAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentId)
            .GreaterThan(0)
            .WithMessage("Assignment ID tidak valid.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Catatan tidak boleh melebihi 500 karakter.");
    }
}