namespace ProcurementSys.Domain.Exceptions;

public class InvalidStatusTransitionException : DomainException
{
    public InvalidStatusTransitionException(string currentStatus, string targetAction)
        : base($"Tidak dapat melakukan aksi '{targetAction}' pada status saat ini: '{currentStatus}'.")
    {
    }
}