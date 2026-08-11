namespace ProcurementSys.Infrastructure.BackgroundJobs;

// Expire token magic-link vendor non-listing yang lewat ExpiredAt tapi belum IsUsed
public class VendorTokenExpiryJob
{
    public Task RunAsync()
    {
        // TODO: implement
        return Task.CompletedTask;
    }
}
