using System;
using System.Collections.Generic;
using Clients.EdmundsApi.Contracts.Extensions;
using Clients.EdmundsApi.Http;
using Clients.EdmundsApi.Resources;
using Microsoft.Extensions.Logging;

namespace Clients.EdmundsApi.Query
{
	internal abstract class BaseQuery<T>
	{
		private const string ApiKeyQueryName = "api_key";
		private const string FmtQueryParameter = "json";
		private const string FmtQueryName = "fmt";

		private string _baseUrl;
		private IConfiguration _configuration;
		private IDictionary<string, string> _queryParameters;

		/// <summary>
		/// Base Url used to construct the desired API endpoint query in <see cref="BaseQuery.Url"/>
		/// </summary>
		protected string BaseUrl
		{
			get { return _baseUrl; }
			set {
				if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
					throw new UriFormatException(ExceptionMessages.InvalidUriArgumentException);
				_baseUrl = value;
			}
		}

		/// <summary>
		/// Used to execute queries against the Edmunds API using an http protocol.
		/// </summary>
		protected IHttpClient HttpClient { get; }

		/// <summary>
		/// Abstract logging instance to be overriden by child inheritors.
		/// </summary>
		protected abstract ILogger<T> Logger { get; }

		/// <summary>
		/// The constructed url based on provided <see cref="BaseQuery.BaseUrl"/> and Query Parameters set by
		/// <see cref="BaseQuery.InitializeQueryParameters"/> and <see cref="BaseQuery.AddQueryParameter(string, string)"/>.
		/// </summary>
		internal string Url
		{
			get {
				string url = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(_baseUrl, _queryParameters);
				Logger.LogDebug(url);
				return url;
			}
		}

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="configuration"><see cref="IConfiguration"/></param>
		/// <param name="httpClient"><see cref="IHttpClient"/></param>
		public BaseQuery(IConfiguration configuration, IHttpClient httpClient)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			_configuration = configuration;
			HttpClient = httpClient;
		}

		/// <summary>
		/// Initializes a QueryParameters Dictionary and seeds it with the ApiKey and Fmt.
		/// This object is used to populate the query parameters and construct <see cref="Url"/>.
		/// Call <see cref="AddQueryParameter(string, string)"/> to include additional query 
		/// parameters.
		/// </summary>
		/// <remarks>
		/// This will erase any currently set query parameters, ensure you call this before 
		/// <see cref="AddQueryParameter(string, string)"/>.
		/// </remarks>
		protected void InitializeQueryParameters()
		{
			_queryParameters = new Dictionary<string, string>();
			AddQueryParameter(FmtQueryName, FmtQueryParameter);
			AddQueryParameter(ApiKeyQueryName, _configuration.ApiKey);
		}

		/// <summary>
		/// Adds a Key/Value pair to the QueryParameters dictionary used to populate the query 
		/// parameters appended to <see cref="Url"/>.
		/// </summary>
		protected void AddQueryParameter(string key, string value) => _queryParameters[key] = value;
	}
}
