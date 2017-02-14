using System.Net.Http;
using System.Threading.Tasks;

namespace Clients.EdmundsApi.Http
{
	internal class EdmundsHttpClient : IHttpClient
	{
		internal readonly HttpClient _httpClient;

		public EdmundsHttpClient()
		{
			_httpClient = new HttpClient();
		}

		internal EdmundsHttpClient(HttpMessageHandler httpMessageHandler)
		{
			_httpClient = new HttpClient(httpMessageHandler);
		}

		/// <summary>
		/// This wraps the HttpClient's GetStringAsync(string) method.
		/// Send a GET request to the specified Uri and return the response body as a string in an asynchronous operation.
		/// </summary>
		/// <param name="uri">The Uri the request is sent to.</param>
		public Task<string> GetStringAsync(string uri) => _httpClient.GetStringAsync(uri);
	}
}
