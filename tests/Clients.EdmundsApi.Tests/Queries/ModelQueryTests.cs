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
	public class ModelQueryTests
	{
		private const string ApiKey = nameof(ApiKey);
		private const string GetStringAsync = nameof(GetStringAsync);
		private const string Make = nameof(Make);

		private readonly IConfiguration _configuration;
		private readonly IHttpClient _httpClient;
		private readonly ILogger<ModelQuery> _logger;

		ModelQuery _sut;

		public ModelQueryTests()
		{
			_configuration = Substitute.For<IConfiguration>();
			_httpClient = Substitute.For<IHttpClient>();
			_logger = Substitute.For<ILogger<ModelQuery>>();
			_httpClient.GetStringAsync(Arg.Any<string>()).Returns(nameof(GetStringAsync));

			_sut = new ModelQuery(_configuration, _httpClient, _logger);
		}

		[Fact]
		public async Task GetAllModelsAsync_InvalidStyleId_ArgumentNullException()
		{
			await Assert.ThrowsAsync<ArgumentNullException>(async () =>
				await _sut.GetAllModelsAsync(string.Empty, DateTime.Now.Year, StateEnum.New, ViewEnum.Basic));
		}

		[Fact]
		public async Task GetAllModelsAsync_UrlIsSet_AreEqual()
		{
			string expected = "https://api.edmunds.com/api/vehicle/v2/Make/models?fmt=json&api_key=&year=2017&state=new&view=basic";

			await _sut.GetAllModelsAsync(Make, DateTime.Now.Year, StateEnum.New, ViewEnum.Basic);
			string actual = _sut.Url;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public async Task GetAllModelsAsync_CallsHttpClient_EqualToUrl()
		{
			await _sut.GetAllModelsAsync(Make, DateTime.Now.Year, StateEnum.New, ViewEnum.Basic);

			await _httpClient.Received(1).GetStringAsync(_sut.Url);
		}

		[Fact]
		public async Task GetAllModelsAsync_ReturnsExpectedModel()
		{
			var expected = await _sut.GetAllModelsAsync(Make, DateTime.Now.Year, StateEnum.New, ViewEnum.Basic);

			Assert.IsAssignableFrom<IModelData>(expected);
		}
	}
}
