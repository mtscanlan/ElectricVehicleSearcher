using System.Threading.Tasks;

namespace Clients.EdmundsApi.Contracts
{
	public interface IStyleQuery
    {
		Task<IStyleData> GetStyleDetailsAsync(string styleId, ViewEnum view);
	}
}
