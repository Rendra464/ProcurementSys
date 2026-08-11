namespace ProcurementSys.Infrastructure.BackgroundJobs;

// Didaftarkan ke Hangfire recurring job:
// RecurringJob.AddOrUpdate<PendingApprovalReminderJob>("pending-approval-reminder", j => j.RunAsync(), Cron.Hourly);
public class PendingApprovalReminderJob
{
    public Task RunAsync()
    {
        // TODO: query Assignments Status=Pending yang udah lewat X jam, publish integration event reminder
        return Task.CompletedTask;
    }
}
