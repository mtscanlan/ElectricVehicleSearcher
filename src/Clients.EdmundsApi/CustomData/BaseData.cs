namespace Clients.EdmundsApi.Data
{
	public class BaseData
	{
		protected string _response;

		public BaseData(string response)
		{
			_response = response;
		}

		public override string ToString()
		{
			return _response;
		}
	}
}
