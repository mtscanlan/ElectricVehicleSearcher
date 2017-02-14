using Clients.EdmundsApi.Data;
using Xunit;

namespace Clients.EdmundsApi.Tests.Contracts.Data
{
	public class ModelDataTests
	{
		private const string TestDataRelativePath = "TestData/ModelsChevrolet.json";

		private ModelData _sut;

		[Fact]
		public void StyleIds_ExpectedDynamicModel_CollectionNotEmpty()
		{
			string expected = System.IO.File.ReadAllText(TestDataRelativePath);
			_sut = new ModelData(expected);

			var styleIds = _sut.StyleIds;

			Assert.NotEmpty(styleIds);
		}
	}
}
