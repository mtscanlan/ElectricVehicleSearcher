using System.Threading.Tasks;

namespace Clients.EdmundsApi.Contracts
{
    public interface IMakeQuery
    {
		Task<IMakeData> GetAllMakesAsync(int year, StateEnum state, ViewEnum view);
	}
}
