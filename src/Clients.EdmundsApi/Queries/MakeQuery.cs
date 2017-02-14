using System.Threading.Tasks;
using Clients.EdmundsApi.Contracts;
using Clients.EdmundsApi.Contracts.Extensions;
using Clients.EdmundsApi.Data;
using Clients.EdmundsApi.Http;
using Clients.EdmundsApi.Resources;
using Microsoft.Extensions.Logging;

namespace Clients.EdmundsApi.Query
{
	internal class MakeQuery : BaseQuery<MakeQuery>, IMakeQuery
	{
		protected override ILogger<MakeQuery> Logger { get; }

		public MakeQuery(IConfiguration configuration, IHttpClient httpClient, ILogger<MakeQuery> logger)
			: base(configuration, httpClient)
		{
			BaseUrl = BaseUrls.MakesUrl;
			Logger = logger;
		}

		/// <summary>
		/// Get a list of all vehicle makes (new, used and future) and their models.
		/// </summary>
		/// <see cref="http://developer.edmunds.com/api-documentation/vehicle/spec_make/v2/01_list_of_makes/api-description.html"/>
		/// <param name="year">The four-digit year of interest</param>
		/// <param name="state">The state of the car make, <see cref="StateEnum"/></param>
		/// <param name="view">The response payload style, <see cref="ViewEnum"/></param>
		public async Task<IMakeData> GetAllMakesAsync(int year, StateEnum state, ViewEnum view)
		{
			Logger.LogDebug($"{nameof(GetAllMakesAsync)}({nameof(year)}:{year}, {nameof(state)}:{state}, {nameof(view)}:{view})");

			InitializeQueryParameters();
			AddQueryParameter(nameof(state), state.ToString().ToLower());
			AddQueryParameter(nameof(view), view.ToString().ToLower());
			AddQueryParameter(nameof(year), year.ToString());

			string response = await HttpClient.GetStringAsync(Url);
			Logger.LogDebug(response);

			return new MakeData(response);
		}
	}
}
