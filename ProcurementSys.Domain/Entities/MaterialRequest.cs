using ProcurementSys.Domain.Common;
using ProcurementSys.Domain.Enums;
using ProcurementSys.Domain.Events;
using ProcurementSys.Domain.Exceptions;

namespace ProcurementSys.Domain.Entities;

public class MaterialRequest : BaseEntity
{
    private readonly List<MaterialRequestItem> _items = new();

    public int Id { get; private set; }
    public string RequestNumber { get; private set; } = default!;
    public int RequesterId { get; private set; }
    public int DepartmentId { get; private set; }
    public string? Description { get; private set; }
    public decimal TotalEstimatedValue { get; private set; }
    public MaterialRequestStatus Status { get; private set; } = MaterialRequestStatus.Draft;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<MaterialRequestItem> Items => _items.AsReadOnly();

    private MaterialRequest() { }

    public static MaterialRequest Create(
        string requestNumber,
        int requesterId,
        int departmentId,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(requestNumber))
        {
            throw new ArgumentException("Nomor Material Request tidak boleh kosong.", nameof(requestNumber));
        }

        if (requesterId <= 0)
        {
            throw new ArgumentException("Requester ID tidak valid.", nameof(requesterId));
        }

        if (departmentId <= 0)
        {
            throw new ArgumentException("Department ID tidak valid.", nameof(departmentId));
        }

        return new MaterialRequest
        {
            RequestNumber = requestNumber,
            RequesterId = requesterId,
            DepartmentId = departmentId,
            Description = description,
            Status = MaterialRequestStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void AddItem(string itemName, decimal quantity, string unit, decimal estimatedUnitPrice)
    {
        if (Status != MaterialRequestStatus.Draft)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), "AddItem");
        }

        _items.Add(new MaterialRequestItem
        {
            ItemName = itemName,
            Quantity = quantity,
            Unit = unit,
            EstimatedUnitPrice = estimatedUnitPrice
        });

        RecalculateTotal();
    }

    public void RemoveItem(MaterialRequestItem item)
    {
        if (Status != MaterialRequestStatus.Draft)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), "RemoveItem");
        }

        _items.Remove(item);
        RecalculateTotal();
    }

    public void RecalculateTotal()
    {
        TotalEstimatedValue = _items.Sum(item => item.EstimatedUnitPrice * item.Quantity);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Submit()
    {
        if (Status != MaterialRequestStatus.Draft)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), "Submit");
        }

        if (!_items.Any())
        {
            throw new EmptyMaterialRequestException(Id);
        }

        Status = MaterialRequestStatus.Submitted;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new MaterialRequestSubmittedDomainEvent(Id));
    }

    public void StartApproval()
    {
        if (Status != MaterialRequestStatus.Submitted)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), "StartApproval");
        }

        Status = MaterialRequestStatus.InApproval;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkFullyApproved()
    {
        if (Status != MaterialRequestStatus.InApproval)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), "MarkFullyApproved");
        }

        Status = MaterialRequestStatus.Approved;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new MaterialRequestFullyApprovedDomainEvent(Id));
    }

    public void MarkRejected()
    {
        if (Status != MaterialRequestStatus.Submitted && Status != MaterialRequestStatus.InApproval)
        {
            throw new InvalidStatusTransitionException(Status.ToString(), "MarkRejected");
        }

        Status = MaterialRequestStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new MaterialRequestRejectedDomainEvent(Id));
    }
}