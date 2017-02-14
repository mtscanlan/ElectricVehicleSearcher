using System.Threading.Tasks;

namespace Clients.EdmundsApi.Http
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string uri);
    }
}
