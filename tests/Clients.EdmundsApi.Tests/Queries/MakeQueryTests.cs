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
	public class MakeQueryTests
	{
		private const string ApiKey = nameof(ApiKey);
		private const string GetStringAsync = nameof(GetStringAsync);

		private readonly IConfiguration _configuration;
		private readonly IHttpClient _httpClient;
		private readonly ILogger<MakeQuery> _logger;

		MakeQuery _sut;

		public MakeQueryTests()
		{
			_configuration = Substitute.For<IConfiguration>();
			_httpClient = Substitute.For<IHttpClient>();
			_logger = Substitute.For<ILogger<MakeQuery>>();

			_sut = new MakeQuery(_configuration, _httpClient, _logger);
		}

		[Fact]
		public async Task GetAllMakesAsync_UrlIsSet_AreEqual()
		{
			string expected = "https://api.edmunds.com/api/vehicle/v2/makes?fmt=json&api_key=&state=new&view=basic&year=2017";

			await _sut.GetAllMakesAsync(DateTime.Now.Year, StateEnum.New, ViewEnum.Basic);
			string actual = _sut.Url;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public async Task GetAllMakesAsync_CallsHttpClient_EqualToUrl()
		{
			await _sut.GetAllMakesAsync(DateTime.Now.Year, StateEnum.New, ViewEnum.Basic);

			await _httpClient.Received(1).GetStringAsync(_sut.Url);
		}

		[Fact]
		public async Task GetAllMakesAsync_ReturnsExpectedModel()
		{
			var expected = await _sut.GetAllMakesAsync(DateTime.Now.Year, StateEnum.New, ViewEnum.Basic);

			Assert.IsAssignableFrom<IMakeData>(expected);
		}
	}
}
