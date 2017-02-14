using Clients.EdmundsApi.Data;
using Xunit;

namespace Clients.EdmundsApi.Tests.Contracts.Data
{
	public class MakeDataTests
	{
		private const string TestDataRelativePath = "TestData/Makes.json";

		private MakeData _sut;

		[Fact]
		public void StyleIds_ExpectedDynamicModel_CollectionNotEmpty()
		{
			string expected = System.IO.File.ReadAllText(TestDataRelativePath);
			_sut = new MakeData(expected);

			var allMakes = _sut.AllMakes;

			Assert.NotEmpty(allMakes);
		}
	}
}
