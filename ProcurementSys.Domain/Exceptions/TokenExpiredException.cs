namespace ProcurementSys.Domain.Exceptions;

public class TokenExpiredException : DomainException
{
    public TokenExpiredException(string token, DateTime expiresAt)
        : base($"Tautan persetujuan vendor '{token}' telah kedaluwarsa pada {expiresAt:yyyy-MM-dd HH:mm:ss} UTC.")
    {
    }
}