namespace ProcurementSys.Domain.Entities;

public class Vendor
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? ContactPerson { get; set; }
    public bool HasAppAccess { get; set; } // true = vendor listing (login app), false = non-listing (akses via email link)
    public bool IsActive { get; set; } = true;
}
