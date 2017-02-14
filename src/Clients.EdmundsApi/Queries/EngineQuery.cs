using System;
using System.Threading.Tasks;
using Clients.EdmundsApi.Contracts;
using Clients.EdmundsApi.Contracts.Extensions;
using Clients.EdmundsApi.Data;
using Clients.EdmundsApi.Http;
using Clients.EdmundsApi.Resources;
using Microsoft.Extensions.Logging;

namespace Clients.EdmundsApi.Query
{
	internal class EngineQuery : BaseQuery<EngineQuery>, IEngineQuery
	{
		protected override ILogger<EngineQuery> Logger { get; }

		public EngineQuery(IConfiguration configuration, IHttpClient httpClient, ILogger<EngineQuery> logger)
			: base(configuration, httpClient)
		{
			Logger = logger;
		}

		/// <summary>
		/// Get list of engines and their details for a specific Edmunds style ID
		/// </summary>
		/// <see cref="http://developer.edmunds.com/api-documentation/vehicle/spec_engine_and_transmission/v2/01_engine_id/api-description.html"/>
		/// <param name="styleId">Edmunds vehicle style ID</param>
		/// <param name="availability">The availability option, <see cref="AvailabilityEnum"/></param>
		/// <param name="name">The name of the engine to query, this is optional and defaults to null.</param>
		public async Task<IEngineData> GetEnginesAsync(string styleId, AvailabilityEnum availability, string name = null)
		{
			Logger.LogDebug($"{nameof(GetEnginesAsync)}({nameof(styleId)}:{styleId}, {nameof(availability)}:{availability}, {nameof(name)}:{name ?? "null"})");

			if (string.IsNullOrEmpty(styleId))
				throw new ArgumentNullException(nameof(styleId));

			BaseUrl = string.Format(BaseUrls.EnginesUrl, styleId);

			InitializeQueryParameters();
			AddQueryParameter(nameof(availability), availability.ToString());

			if (!String.IsNullOrWhiteSpace(name))
				AddQueryParameter(nameof(name), name);

			string response = await HttpClient.GetStringAsync(Url);
			Logger.LogDebug(response);

			return new EngineData(response, styleId);
		}
	}
}
