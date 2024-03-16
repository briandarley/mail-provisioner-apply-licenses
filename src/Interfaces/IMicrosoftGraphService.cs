public interface IMicrosoftGraphService
{
    Task<List<UNC.MicrosoftGraph.Common.Entities.Licenses.SubscribedSku>> GetSubscribedSkus(string userPrincipalName);
}