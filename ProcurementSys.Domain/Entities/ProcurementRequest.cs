using ProcurementSys.Domain.Common;
using ProcurementSys.Domain.Enums;
using ProcurementSys.Domain.Events;
using ProcurementSys.Domain.Exceptions;

namespace ProcurementSys.Domain.Entities;

public class ProcurementRequest : BaseEntity
{
    public string ProcurementNumber { get; private set; } = string.Empty;
    public int MaterialRequestId { get; private set; }
    public int? VendorId { get; private set; }
    public ProcurementRequestType? Type { get; private set; }
    public ProcurementRequestStatus Status { get; private set; }
    public MaterialRequest? MaterialRequest { get; private set; }
    public Vendor? Vendor { get; private set; }

    private ProcurementRequest() { }

    public static ProcurementRequest CreatePlaceholder(string procurementNumber, int materialRequestId)
    {
        if (string.IsNullOrWhiteSpace(procurementNumber))
        {
            throw new ArgumentException("Nomor Procurement tidak boleh kosong.", nameof(procurementNumber));
        }

        if (materialRequestId <= 0)
        {
            throw new ArgumentException("Material Request ID tidak valid.", nameof(materialRequestId));
        }

        return new ProcurementRequest
        {
            ProcurementNumber = procurementNumber,
            MaterialRequestId = materialRequestId,
            Status = ProcurementRequestStatus.PendingVendorSelection,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void AssignVendor(Vendor vendor)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        if (Status != ProcurementRequestStatus.PendingVendorSelection)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), nameof(AssignVendor));
        }

        VendorId = vendor.Id;
        Type = vendor.HasAppAccess ? ProcurementRequestType.Listing : ProcurementRequestType.NonListing;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new ProcurementRequestVendorAssignedDomainEvent(Id));
    }

    public void StartApproval()
    {
        if (Status != ProcurementRequestStatus.PendingVendorSelection)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), nameof(StartApproval));
        }

        Status = ProcurementRequestStatus.InApproval;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkFullyApproved()
    {
        if (Status != ProcurementRequestStatus.InApproval)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), nameof(MarkFullyApproved));
        }

        Status = ProcurementRequestStatus.Approved;
        UpdatedAt = DateTime.UtcNow;

        if (Type.HasValue)
        {
            RaiseDomainEvent(new ProcurementRequestApprovedDomainEvent(Id, Type.Value.ToString()));
        }
    }

    public void MarkRejected()
    {
        if (Status != ProcurementRequestStatus.PendingVendorSelection && Status != ProcurementRequestStatus.InApproval)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), nameof(MarkRejected));
        }

        Status = ProcurementRequestStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new ProcurementRequestRejectedDomainEvent(Id));
    }
}