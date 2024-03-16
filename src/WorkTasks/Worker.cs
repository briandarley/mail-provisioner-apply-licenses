using Microsoft.Extensions.Hosting;
using MailProvisionerApplyLicenses.Interfaces;
namespace MailProvisionerApplyLicenses.WorkTasks;

public class Worker : BackgroundService
{
    private IWorkerTask _workerTask;

    public Worker(IWorkerTask workerTask)
    {
        _workerTask = workerTask;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _workerTask.ExecuteAsync(stoppingToken);
    }
}
