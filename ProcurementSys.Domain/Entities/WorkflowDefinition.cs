using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Domain.Entities;

public class WorkflowDefinition
{
    public int Id { get; set; }
    public WorkflowProcessType EntityType { get; set; }
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public List<WorkflowStep> Steps { get; set; } = new();
}
