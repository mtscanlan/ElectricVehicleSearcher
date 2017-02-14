using Clients.EdmundsApi.Contracts;

namespace Clients.EdmundsApi.Data
{
	public class StyleData : BaseData, IStyleData
	{
		public StyleData(string response) : base(response) { }
	}
}
