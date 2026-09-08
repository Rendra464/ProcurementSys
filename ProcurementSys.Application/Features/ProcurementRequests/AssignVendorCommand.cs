using MediatR;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Application.Common.Exceptions;
using ProcurementSys.Domain.Exceptions;

namespace ProcurementSys.Application.Features.ProcurementRequests;

public record AssignVendorCommand(int ProcurementRequestId, int VendorId) : IRequest;

public class AssignVendorCommandHandler : IRequestHandler<AssignVendorCommand>
{
    private readonly IProcurementRequestRepository _procurementRequestRepository;
    private readonly IVendorRepository _vendorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignVendorCommandHandler(
        IProcurementRequestRepository procurementRequestRepository,
        IVendorRepository vendorRepository,
        IUnitOfWork unitOfWork)
    {
        _procurementRequestRepository = procurementRequestRepository;
        _vendorRepository = vendorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AssignVendorCommand request, CancellationToken cancellationToken)
    {
        var procurementRequest = await _procurementRequestRepository.GetByIdAsync(request.ProcurementRequestId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.ProcurementRequest), request.ProcurementRequestId);

        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Vendor), request.VendorId);

        procurementRequest.AssignVendor(vendor);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}