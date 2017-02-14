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
	internal class ModelQuery : BaseQuery<ModelQuery>, IModelQuery
	{
		protected override ILogger<ModelQuery> Logger { get; }

		public ModelQuery(IConfiguration configuration, IHttpClient httpClient, ILogger<ModelQuery> logger)
			: base(configuration, httpClient)
		{
			Logger = logger;
		}

		/// <summary>
		/// Get a list of car models for a specific car make by the make's niceName.
		/// </summary>
		/// <see cref="http://developer.edmunds.com/api-documentation/vehicle/spec_model/v2/01_list_of_models/api-description.html"/>
		/// <param name="make">The makes nicename in which the method will query</param>
		/// <param name="year">The four-digit year of interest</param>
		/// <param name="state">The state of the car make, <see cref="StateEnum"/></param>
		/// <param name="view">The response payload style, <see cref="ViewEnum"/></param>
		public async Task<IModelData> GetAllModelsAsync(string make, int year, StateEnum state, ViewEnum view)
		{
			Logger.LogDebug($"{nameof(GetAllModelsAsync)}({nameof(make)}:{make}, {nameof(year)}:{year}, {nameof(state)}:{state}, {nameof(view)}:{view})");

			if (string.IsNullOrEmpty(make))
				throw new ArgumentNullException(nameof(make));

			BaseUrl = string.Format(BaseUrls.ModelsUrl, make);

			InitializeQueryParameters();
			AddQueryParameter(nameof(year), year.ToString());
			AddQueryParameter(nameof(state), state.ToString().ToLower());
			AddQueryParameter(nameof(view), view.ToString().ToLower());

			string response = await HttpClient.GetStringAsync(Url);
			Logger.LogDebug(response);

			return new ModelData(response);
		}
	}
}
