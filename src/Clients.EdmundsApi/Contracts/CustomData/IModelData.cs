using System.Collections.Generic;

namespace Clients.EdmundsApi.Contracts
{
	public interface IModelData
	{
		IEnumerable<string> StyleIds { get; }
	}
}
