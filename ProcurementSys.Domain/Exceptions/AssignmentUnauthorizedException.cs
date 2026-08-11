namespace ProcurementSys.Domain.Exceptions;

public class AssignmentUnauthorizedException : DomainException
{
    public AssignmentUnauthorizedException(int userId, int assignmentId, int? assigneeUserId)
        : base($"User ID {userId} tidak berhak memproses Assignment ID {assignmentId} (Assignee terdaftar: {assigneeUserId?.ToString() ?? "None"}).")
    {
    }
}