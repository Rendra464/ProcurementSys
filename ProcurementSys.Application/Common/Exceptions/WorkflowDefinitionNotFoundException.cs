using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Application.Common.Exceptions;
public class WorkflowDefinitionNotFoundException : Exception
{
    public WorkflowDefinitionNotFoundException(WorkflowProcessType entityType)
        : base($"Definisi workflow aktif untuk tipe proses '{entityType}' tidak ditemukan di sistem.")
    {
    }
}