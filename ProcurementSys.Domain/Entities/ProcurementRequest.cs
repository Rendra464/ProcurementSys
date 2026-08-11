using ProcurementSys.Domain.Common;
using ProcurementSys.Domain.Enums;
using ProcurementSys.Domain.Events;
using ProcurementSys.Domain.Exceptions;

namespace ProcurementSys.Domain.Entities;

public class ProcurementRequest : BaseEntity
{
    private ProcurementRequest() { }

    public int Id { get; set; }
    public string ProcurementNumber { get; private set; } = default!;
    public int MaterialRequestId { get; private set; }
    public int VendorId { get; private set; }
    public ProcurementRequestType Type { get; private set; }
    public ProcurementRequestStatus Status { get; private set; } = ProcurementRequestStatus.Draft;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public static ProcurementRequest CreateForVendor(
        string procurementNumber,
        int materialRequestId,
        Vendor vendor)
    {
        var type = vendor.HasAppAccess
            ? ProcurementRequestType.Listing
            : ProcurementRequestType.NonListing;

        return new ProcurementRequest
        {
            ProcurementNumber = procurementNumber,
            MaterialRequestId = materialRequestId,
            VendorId = vendor.Id,
            Type = type,
            Status = ProcurementRequestStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void StartApproval()
    {
        if (Status != ProcurementRequestStatus.Draft)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), "StartApproval");
        }

        Status = ProcurementRequestStatus.InApproval;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkFullyApproved()
    {
        if (Status != ProcurementRequestStatus.InApproval)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), "MarkFullyApproved");
        }

        Status = ProcurementRequestStatus.Approved;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new ProcurementRequestApprovedDomainEvent(Id, Type.ToString()));
    }

    public void MarkRejected()
    {
        if (Status != ProcurementRequestStatus.Draft && Status != ProcurementRequestStatus.InApproval)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), "MarkRejected");
        }

        Status = ProcurementRequestStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new ProcurementRequestRejectedDomainEvent(Id));
    }
}