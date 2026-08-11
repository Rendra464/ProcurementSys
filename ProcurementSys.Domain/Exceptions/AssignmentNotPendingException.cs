namespace ProcurementSys.Domain.Exceptions;

public class AssignmentNotPendingException : DomainException
{
    public AssignmentNotPendingException(int assignmentId, string currentStatus)
        : base($"Assignment ID {assignmentId} sudah tidak berstatus Pending (Status saat ini: {currentStatus}).")
    {
    }
}