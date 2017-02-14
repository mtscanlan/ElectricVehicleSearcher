using Clients.EdmundsApi.Data;
using Xunit;

namespace Clients.EdmundsApi.Tests.Contracts.Data
{
	public class EngineDataTests
	{
		private const string TestDataRelativePath = "TestData/EnginesBolt.json";
		private const string StyleId = nameof(StyleId);

		private EngineData _sut;

		[Fact]
		public void StyleId_SetByCtor_Equal()
		{
			string testData = System.IO.File.ReadAllText(TestDataRelativePath);
			_sut = new EngineData(testData, StyleId);

			string actual = _sut.StyleId;

			Assert.Equal(expected: StyleId, actual: actual);
		}

		[Fact]
		public void IsElectric_DynamicParsesTestData_ReturnsTrue()
		{
			string testData = System.IO.File.ReadAllText(TestDataRelativePath);
			_sut = new EngineData(testData, StyleId);

			var actual = _sut.IsElectric;

			Assert.True(actual);
		}
	}
}
