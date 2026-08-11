using ProcurementSys.Domain.Enums;
using ProcurementSys.Domain.Exceptions;

namespace ProcurementSys.Domain.Entities;

public class VendorApprovalToken
{
    public int Id { get; set; }
    public string Token { get; private set; } = default!;
    public int ProcurementRequestId { get; private set; }
    public int VendorId { get; private set; }
    public int AssignmentId { get; private set; }
    public string VendorEmail { get; private set; } = default!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }
    public DateTime? UsedAt { get; private set; }
    public AssignmentStatus? Decision { get; private set; }
    public string? DecisionNotes { get; private set; }

    private VendorApprovalToken() { }

    /// <summary>
    /// Factory Method untuk membuat token baru yang aman (GUID-based) dengan durasi kedaluwarsa tertentu.
    /// </summary>
    public static VendorApprovalToken Generate(
        int procurementRequestId,
        int vendorId,
        int assignmentId,
        string vendorEmail,
        TimeSpan validDuration)
    {
        return new VendorApprovalToken
        {
            Token = Guid.NewGuid().ToString("N"),
            ProcurementRequestId = procurementRequestId,
            VendorId = vendorId,
            AssignmentId = assignmentId,
            VendorEmail = vendorEmail,
            ExpiresAt = DateTime.UtcNow.Add(validDuration),
            IsUsed = false,
            Decision = null
        };
    }

    /// <summary>
    /// Mengunci token setelah digunakan oleh Vendor Non-Listing untuk Approve/Reject (BR04).
    /// </summary>
    public void MarkAsUsed(AssignmentStatus decision, string? notes = null)
    {
        // Guard input parameter: Keputusan vendor HANYA boleh Approved atau Rejected
        if (decision != AssignmentStatus.Approved && decision != AssignmentStatus.Rejected)
        {
            throw new ArgumentException("Keputusan vendor hanya boleh berupa Approved atau Rejected.", nameof(decision));
        }

        EnsureValidToUse();

        IsUsed = true;
        UsedAt = DateTime.UtcNow;
        Decision = decision;
        DecisionNotes = notes;
    }

    /// <summary>
    /// Guard Clause BR04: Memastikan token belum digunakan dan belum kedaluwarsa.
    /// </summary>
    public void EnsureValidToUse()
    {
        if (IsUsed)
        {
            throw new TokenAlreadyUsedException(Token);
        }

        if (DateTime.UtcNow > ExpiresAt)
        {
            throw new TokenExpiredException(Token, ExpiresAt);
        }
    }
}