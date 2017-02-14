using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clients.EdmundsApi.Contracts;
using Clients.EdmundsApi.Contracts.Extensions;
using Clients.EdmundsApi.Http;
using Clients.EdmundsApi.Query;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Clients.EdmundsApi.Tests.Queries
{
	public class StyleQueryTests
	{
		private const string ApiKey = nameof(ApiKey);
		private const string GetStringAsync = nameof(GetStringAsync);
		private const string StyleId = nameof(StyleId);

		private readonly IConfiguration _configuration;
		private readonly IHttpClient _httpClient;
		private readonly ILogger<StyleQuery> _logger;

		StyleQuery _sut;

		public StyleQueryTests()
		{
			_configuration = Substitute.For<IConfiguration>();
			_httpClient = Substitute.For<IHttpClient>();
			_logger = Substitute.For<ILogger<StyleQuery>>();

			_sut = new StyleQuery(_configuration, _httpClient, _logger);
		}

		[Fact]
		public async Task GetStyleDetailsAsync_InvalidStyleId_ArgumentNullException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
				await _sut.GetStyleDetailsAsync(string.Empty, ViewEnum.Basic));
		}

		[Fact]
		public async Task GetStyleDetailsAsync_UrlIsSet_AreEqual()
		{
			string expected = "https://api.edmunds.com/api/vehicle/v2/styles/StyleId?fmt=json&api_key=&view=basic";

			await _sut.GetStyleDetailsAsync(StyleId, ViewEnum.Basic);
			string actual = _sut.Url;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public async Task GetStyleDetailsAsync_CallsHttpClient_EqualToUrl()
		{
			await _sut.GetStyleDetailsAsync(StyleId, ViewEnum.Basic);

			await _httpClient.Received(1).GetStringAsync(_sut.Url);
		}

		[Fact]
		public async Task GetAllModelsAsync_ReturnsExpectedModel()
		{
			var expected = await _sut.GetStyleDetailsAsync(StyleId, ViewEnum.Basic);

			Assert.IsAssignableFrom<IStyleData>(expected);
		}
	}
}
