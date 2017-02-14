using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Clients.EdmundsApi.Http
{
	internal class EdmundsHttpClientTests
	{
		private class MockMessageHandler : HttpMessageHandler
		{
			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				return Task.FromResult(
					new HttpResponseMessage() {
						Content = new StringContent(ResponseMessage)
					});
			}
		}

		private const string FakeDomain = "http://www.fakeurl.com";
		private const string ResponseMessage = nameof(ResponseMessage);

		private EdmundsHttpClient _sut;

		[Fact]
		public void GetStringAsync_ReturnsString_Equal()
		{
			string expected = ResponseMessage;

			_sut = new EdmundsHttpClient(new MockMessageHandler());
			string actual = _sut.GetStringAsync(FakeDomain).Result;

			Assert.Equal(expected, actual);
		}
	}
}
