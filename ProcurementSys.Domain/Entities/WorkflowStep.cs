using ProcurementSys.Domain.Enums;
namespace ProcurementSys.Domain.Entities;

public class WorkflowStep
{
    public int Id { get; set; }
    public int WorkflowDefinitionId { get; set; }
    public int StepOrder { get; set; }
    public string StepName { get; set; } = default!;
    public UserRole RequiredRole { get; set; }
    public string? ConditionJson { get; set; }
}
