using FluentValidation;

namespace ProcurementSys.Application.Features.MaterialRequests.Validators;

public class CreateMaterialRequestCommandValidator : AbstractValidator<CreateMaterialRequestCommand>
{
    public CreateMaterialRequestCommandValidator()
    {
        RuleFor(x => x.RequesterId)
            .GreaterThan(0)
            .WithMessage("RequesterId tidak valid.");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage("DepartmentId tidak valid.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Material Request harus memiliki minimal satu item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ItemName)
                .NotEmpty()
                .WithMessage("Nama item tidak boleh kosong.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0)
                .WithMessage("Jumlah item (Quantity) harus lebih besar dari 0.");

            item.RuleFor(i => i.Unit)
                .NotEmpty()
                .WithMessage("Satuan item (Unit) tidak boleh kosong.");

            item.RuleFor(i => i.EstimatedUnitPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Estimasi harga satuan tidak boleh negatif.");
        });
    }
}