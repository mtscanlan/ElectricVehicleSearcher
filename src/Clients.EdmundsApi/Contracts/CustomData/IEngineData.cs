using System.Collections.Generic;

namespace Clients.EdmundsApi.Contracts
{
	public interface IEngineData
	{
		string StyleId { get; }
		bool IsElectric { get; }
	}
}
