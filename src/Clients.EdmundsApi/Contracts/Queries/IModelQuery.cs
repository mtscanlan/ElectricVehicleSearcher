using System.Threading.Tasks;

namespace Clients.EdmundsApi.Contracts
{
	public interface IModelQuery
    {
		Task<IModelData> GetAllModelsAsync(string make, int year, StateEnum state, ViewEnum view);
	}
}
