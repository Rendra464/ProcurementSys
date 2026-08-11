using MediatR;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Application.Common.Exceptions;
using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Application.Features.MaterialRequests;

public record SubmitMaterialRequestCommand(int MaterialRequestId) : IRequest;

public class SubmitMaterialRequestCommandHandler : IRequestHandler<SubmitMaterialRequestCommand>
{
    private readonly IMaterialRequestRepository _materialRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitMaterialRequestCommandHandler(
        IMaterialRequestRepository materialRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _materialRequestRepository = materialRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SubmitMaterialRequestCommand request, CancellationToken cancellationToken)
    {
        var materialRequest = await _materialRequestRepository.GetByIdAsync(
            request.MaterialRequestId,
            cancellationToken);

        if (materialRequest is null)
        {
            throw new NotFoundException(nameof(MaterialRequest), request.MaterialRequestId);
        }
        materialRequest.Submit();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}