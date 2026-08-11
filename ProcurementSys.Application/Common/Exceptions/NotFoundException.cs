namespace ProcurementSys.Application.Common.Exceptions;
public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entitas \"{name}\" dengan kunci ('{key}') tidak ditemukan.")
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }
}