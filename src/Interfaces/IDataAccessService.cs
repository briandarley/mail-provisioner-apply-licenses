using UNC.Services.Responses;

public interface IDataAccessService
{
    Task<PagedResponse<UNC.DataAccessAPI.Common.Entities.MailProvisionDb.UserAccount>> GetUserMailboxRequests(UNC.DataAccessAPI.Common.Criteria.MailProvisionDb.UserAccountsCriteria criteria);
    Task AddAppliedLicense(int pid, string SkuPartNumber);

}