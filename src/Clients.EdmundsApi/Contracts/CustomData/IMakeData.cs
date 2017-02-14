using System.Collections.Generic;

namespace Clients.EdmundsApi.Contracts
{
	public interface IMakeData
	{
		IEnumerable<string> AllMakes { get; }
	}
}
