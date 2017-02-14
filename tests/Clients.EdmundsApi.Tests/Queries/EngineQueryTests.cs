using System;
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
	public class EngineQueryTests
	{
		private const string ApiKey = nameof(ApiKey);
		private const string GetStringAsync = nameof(GetStringAsync);
		private const string StyleId = nameof(StyleId);

		private readonly IConfiguration _configuration;
		private readonly IHttpClient _httpClient;
		private readonly ILogger<EngineQuery> _logger;

		EngineQuery _sut;

		public EngineQueryTests()
		{
			_configuration = Substitute.For<IConfiguration>();
			_httpClient = Substitute.For<IHttpClient>();
			_logger = Substitute.For<ILogger<EngineQuery>>();

			_sut = new EngineQuery(_configuration, _httpClient, _logger);
		}

		[Fact]
		public async Task GetEnginesAsync_InvalidStyleId_ArgumentNullException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
				await _sut.GetEnginesAsync(string.Empty, AvailabilityEnum.Standard));
		}

		[Fact]
		public async Task GetEnginesAsync_UrlIsSet_AreEqual()
		{
			string expected = "https://api.edmunds.com/api/vehicle/v2/styles/StyleId/engines?fmt=json&api_key=&availability=Standard";

			await _sut.GetEnginesAsync(StyleId, AvailabilityEnum.Standard);
			string actual = _sut.Url;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public async Task GetEnginesAsync_UrlIsSetWithName_AreEqual()
		{
			string expected = "https://api.edmunds.com/api/vehicle/v2/styles/StyleId/engines?fmt=json&api_key=&availability=Standard&name=Chevrolet";

			await _sut.GetEnginesAsync(StyleId, AvailabilityEnum.Standard, "Chevrolet");
			string actual = _sut.Url;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public async Task GetEnginesAsync_CallsHttpClient_Once()
		{
			await _sut.GetEnginesAsync(StyleId, AvailabilityEnum.Standard);

			await _httpClient.Received(1).GetStringAsync(Arg.Any<string>());
		}

		[Fact]
		public async Task GetEnginesAsync_ReturnsExpectedModel()
		{
			var expected = await _sut.GetEnginesAsync(StyleId, AvailabilityEnum.Standard);

			Assert.IsAssignableFrom<IEngineData>(expected);
		}
	}
}
