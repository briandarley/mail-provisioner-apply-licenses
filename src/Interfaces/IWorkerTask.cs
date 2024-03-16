namespace MailProvisionerApplyLicenses.Interfaces;

public interface IWorkerTask
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}