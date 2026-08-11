namespace ProcurementSys.Domain.Entities;

public class MaterialRequestItem
{
    public int Id { get; set; }
    public int MaterialRequestId { get; set; }
    public string ItemName { get; set; } = default!;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = default!;
    public decimal EstimatedUnitPrice { get; set; }
}
