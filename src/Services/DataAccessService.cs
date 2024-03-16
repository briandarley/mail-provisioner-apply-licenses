using UNC.Services.Responses;
using UNC.Extensions.General;
using System.Text;
public class DataAccessService : IDataAccessService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DataAccessService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<PagedResponse<UNC.DataAccessAPI.Common.Entities.MailProvisionDb.UserAccount>> GetUserMailboxRequests(UNC.DataAccessAPI.Common.Criteria.MailProvisionDb.UserAccountsCriteria criteria)
    {

        var client = _httpClientFactory.CreateClient("LOCAL_DATA");

        var request = new HttpRequestMessage(HttpMethod.Get, $"mail-provision-db/user-accounts?{criteria.ToQueryParams()}");
        var response = await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var data = System.Text.Json.JsonSerializer.Deserialize<PagedResponse<UNC.DataAccessAPI.Common.Entities.MailProvisionDb.UserAccount>>(content, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return data;
        }
        return null;
    }

    public async Task AddAppliedLicense(int pid, string SkuPartNumber)
    {


        var appliedLicense = new UNC.DataAccessAPI.Common.Entities.MailProvisionDb.AppliedLicense
        {
            UserAccountId = pid,
            SkuPartNumber = SkuPartNumber,
        };
        var client = _httpClientFactory.CreateClient("LOCAL_DATA");
        var request = new HttpRequestMessage(HttpMethod.Post, $"mail-provision-db/user-accounts/{appliedLicense.UserAccountId}/applied-licenses");
        request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(appliedLicense), Encoding.UTF8, "application/json");
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to update user account");
        }
    }
}