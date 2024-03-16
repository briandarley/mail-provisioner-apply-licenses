using MailProvisionerApplyLicenses.Interfaces;

namespace MailProvisionerApplyLicenses.WorkTasks;

public class WorkerTask : IWorkerTask
{
    private readonly IMicrosoftGraphService _microsoftGraphService;
    private readonly IDataAccessService _dataAccessService;

    public WorkerTask(IMicrosoftGraphService microsoftGraphService, IDataAccessService dataAccessService)
    {
        _microsoftGraphService = microsoftGraphService;
        _dataAccessService = dataAccessService;
    }


    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Your background task logic here
            Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");



            // foreach (var item in request)
            // {
            //     Console.WriteLine(item.SkuPartNumber);
            // }

            var criteria = new UNC.DataAccessAPI.Common.Criteria.MailProvisionDb.UserAccountsCriteria() { PageSize = 100, Index = 0 };
            var dataRequest = await _dataAccessService.GetUserMailboxRequests(criteria);
            do
            {

                dataRequest = await _dataAccessService.GetUserMailboxRequests(criteria);


                foreach (var item in dataRequest.Entities)
                {
                    if (item.AppliedLicenses.Count == 0)
                    {


                        var request = await _microsoftGraphService.GetSubscribedSkus($"{item.Uid}@ad.unc.edu");
                        if (request != null)
                        {
                            var licenses = request.Where(c => c.SkuPartNumber.Contains("_A3_"));

                            if (licenses.Count() == 0)
                            {
                                await _dataAccessService.AddAppliedLicense(item.Pid, "NONE");
                                Console.WriteLine($"No licenses found for {item.Uid}");
                                continue;
                            }

                            foreach (var lic in licenses)
                            {
                                await _dataAccessService.AddAppliedLicense(item.Pid, lic.SkuPartNumber);
                                Console.WriteLine($"Added license {lic.SkuPartNumber} to {item.Uid}");
                            }
                        }
                        else
                        {
                            await _dataAccessService.AddAppliedLicense(item.Pid, "NONE");
                            Console.WriteLine($"No licenses found for {item.Uid}");
                        }


                    }

                }


                criteria.Index++;
            } while (dataRequest is not null && dataRequest.Entities != null && dataRequest.Entities.Count() > 0);

        }














        //await Task.Delay(1000, stoppingToken); // Wait for 1 second
    }
}
