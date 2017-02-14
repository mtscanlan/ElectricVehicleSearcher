using System.Threading.Tasks;

namespace Clients.EdmundsApi.Contracts
{
	public interface IEngineQuery
    {
		Task<IEngineData> GetEnginesAsync(string styleId, AvailabilityEnum availability, string name = null);
	}
}
