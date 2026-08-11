using System.Text.Json;
using ProcurementSys.Domain.Entities;

namespace ProcurementSys.Domain.Services;

public class ApprovalWorkflowService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Mengevaluasi apakah suatu WorkflowStep memenuhi syarat untuk dieksekusi 
    /// berdasarkan kondisi bisnis (misal: threshold nilai MinAmount).
    /// </summary>
    public bool IsStepConditionMet(WorkflowStep step, decimal materialRequestValue)
    {
        if (string.IsNullOrWhiteSpace(step.ConditionJson))
        {
            return true;
        }

        try
        {
            var condition = JsonSerializer.Deserialize<ApprovalConditionDto>(step.ConditionJson, JsonOptions);

            // Jika ada MinAmount yang ditentukan di JSON, bandingkan dengan nilai total MR (BR02)
            if (condition?.MinAmount is decimal minAmount)
            {
                return materialRequestValue >= minAmount;
            }

            // Jika JSON terdefinisi tapi tidak mengandung MinAmount, jalankan step
            return true;
        }
        catch (JsonException)
        {
            // Fallback aman jika JSON tidak valid: jalankan step demi keamanan alur approval
            return true;
        }
    }

    /// <summary>
    /// Mencari step berikutnya yang valid dari WorkflowDefinition setelah currentStepOrder.
    /// Otomatis melewati (skip) step yang kondisi bisnisnya tidak terpenuhi.
    /// </summary>
    public WorkflowStep? GetNextStep(
        WorkflowDefinition definition,
        int currentStepOrder,
        decimal materialRequestValue)
    {
        var nextCandidateSteps = definition.Steps
            .Where(s => s.StepOrder > currentStepOrder)
            .OrderBy(s => s.StepOrder);

        foreach (var step in nextCandidateSteps)
        {
            if (IsStepConditionMet(step, materialRequestValue))
            {
                return step;
            }
        }

        return null;
    }
}