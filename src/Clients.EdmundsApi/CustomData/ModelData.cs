using System.Collections.Generic;
using Clients.EdmundsApi.Contracts;
using Newtonsoft.Json;

namespace Clients.EdmundsApi.Data
{
	public class ModelData : BaseData, IModelData
	{
		public IEnumerable<string> StyleIds
		{
			get {
				dynamic models = JsonConvert.DeserializeObject<dynamic>(_response).models;
				foreach (dynamic model in models)
					foreach (dynamic modelYear in model.years)
						foreach (dynamic makeYearStyle in modelYear.styles)
							yield return makeYearStyle.id;
			}
		}

		public ModelData(string response) : base(response) { }
	}
}
