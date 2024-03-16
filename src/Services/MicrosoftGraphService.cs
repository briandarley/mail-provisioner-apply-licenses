using UNC.MicrosoftGraph.Common.Entities.Licenses;
using UNC.MicrosoftGraph.Common.ListResults;
using UNC.MicrosoftGraph.Common.Pagination;
public class MicrosoftGraphService : IMicrosoftGraphService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MicrosoftGraphService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<UNC.MicrosoftGraph.Common.Entities.Licenses.SubscribedSku>> GetSubscribedSkus(string userPrincipalName)
    {
        var result = new List<UNC.MicrosoftGraph.Common.Entities.Licenses.SubscribedSku>();

        var client = _httpClientFactory.CreateClient("LOCAL_MS_GRAPH");
        var request = new HttpRequestMessage(HttpMethod.Get, $"users/{userPrincipalName}/licenses");
        var response = await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var data = System.Text.Json.JsonSerializer.Deserialize<ListResult<SubscribedSku>>(content, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            result = data.Value.ToList();
        }
        return result;


    }
}