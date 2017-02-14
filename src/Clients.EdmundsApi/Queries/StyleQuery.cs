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
	internal class StyleQuery : BaseQuery<StyleQuery>, IStyleQuery
	{
		protected override ILogger<StyleQuery> Logger { get; }

		public StyleQuery(IConfiguration configuration, IHttpClient httpClient, ILogger<StyleQuery> logger)
			: base(configuration, httpClient)
		{
			Logger = logger;
		}

		/// <summary>
		/// Get vehicle style details by Edmunds vehicle style ID.
		/// </summary>
		/// <see cref="http://developer.edmunds.com/api-documentation/vehicle/spec_style/v2/02_by_id/api-description.html"/>
		/// <param name="styleId">Edmunds vehicle style ID</param>
		/// <param name="view">The response payload style, <see cref="ViewEnum"/></param>
		public async Task<IStyleData> GetStyleDetailsAsync(string styleId, ViewEnum view)
		{
			Logger.LogDebug($"{nameof(GetStyleDetailsAsync)}({nameof(styleId)}:{styleId}, {nameof(view)}:{view})");

			if (string.IsNullOrEmpty(styleId))
				throw new ArgumentNullException(nameof(styleId));

			BaseUrl = string.Format(BaseUrls.StylesUrl, styleId);

			InitializeQueryParameters();
			AddQueryParameter(nameof(view), view.ToString().ToLower());

			string response = await HttpClient.GetStringAsync(Url);
			Logger.LogDebug(response);

			return new StyleData(response);
		}
	}
}
