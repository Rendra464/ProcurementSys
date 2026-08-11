namespace ProcurementSys.Domain.Exceptions;

public class EmptyMaterialRequestException : DomainException
{
    public EmptyMaterialRequestException(int materialRequestId)
        : base($"Material Request ID {materialRequestId} tidak boleh kosong/tanpa item saat di-submit.")
    {
    }
}