using Clients.EdmundsApi.Data;
using Xunit;

namespace Clients.EdmundsApi.Tests.Contracts.Data
{
	public class StyleDataTests
	{
		private const string TestDataRelativePath = "TestData/StylesBolt.json";

		private StyleData _sut;

		[Fact]
		public void ToString_StringifySerializerObject_EqualToResponse()
		{
			string expected = System.IO.File.ReadAllText(TestDataRelativePath);
			_sut = new StyleData(expected);

			string actual = _sut.ToString();

			Assert.Equal(expected, actual);
		}
	}
}
