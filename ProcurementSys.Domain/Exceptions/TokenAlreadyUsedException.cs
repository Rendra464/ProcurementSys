namespace ProcurementSys.Domain.Exceptions;

public class TokenAlreadyUsedException : DomainException
{
    public TokenAlreadyUsedException(string token)
        : base($"Tautan persetujuan vendor '{token}' sudah pernah digunakan sebelumnya.")
    {
    }
}