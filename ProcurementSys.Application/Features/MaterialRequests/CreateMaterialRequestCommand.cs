using MediatR;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Application.Abstractions.Services;
using ProcurementSys.Application.Features.MaterialRequests.Dtos;
using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Application.Features.MaterialRequests;

public record CreateMaterialRequestCommand(
    int RequesterId,
    int DepartmentId,
    string? Description,
    List<CreateMaterialRequestItemDto> Items
) : IRequest<int>;

public class CreateMaterialRequestCommandHandler : IRequestHandler<CreateMaterialRequestCommand, int>
{
    private readonly IMaterialRequestRepository _materialRequestRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMaterialRequestCommandHandler(
        IMaterialRequestRepository materialRequestRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        _materialRequestRepository = materialRequestRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateMaterialRequestCommand request, CancellationToken cancellationToken)
    {
        var requestNumber = GenerateRequestNumber();

        var materialRequest = MaterialRequest.Create(
            requestNumber,
            request.RequesterId,
            request.DepartmentId,
            request.Description);

        foreach (var item in request.Items)
        {
            materialRequest.AddItem(
                item.ItemName,
                item.Quantity,
                item.Unit,
                item.EstimatedUnitPrice);
        }

        await _materialRequestRepository.AddAsync(materialRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return materialRequest.Id;
    }

    private string GenerateRequestNumber()
    {
        var datePart = _dateTimeProvider.UtcNow.ToString("yyyyMMdd");
        var uniquePart = Guid.NewGuid().ToString("N")[..4].ToUpper();
        return $"MR-{datePart}-{uniquePart}";
    }
}