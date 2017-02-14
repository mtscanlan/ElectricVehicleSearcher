using System.Collections.Generic;
using Clients.EdmundsApi.Contracts;
using Newtonsoft.Json;

namespace Clients.EdmundsApi.Data
{
	public class MakeData : BaseData, IMakeData
	{
		public IEnumerable<string> AllMakes
		{
			get {
				dynamic makes = JsonConvert.DeserializeObject<dynamic>(_response).makes;
				foreach (dynamic make in makes)
					yield return make.niceName;
			}
		}

		public MakeData(string response) : base(response) { }
	}
}
