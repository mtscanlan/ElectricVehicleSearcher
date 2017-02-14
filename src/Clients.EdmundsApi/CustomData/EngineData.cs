using System;
using Clients.EdmundsApi.Contracts;
using Newtonsoft.Json;

namespace Clients.EdmundsApi.Data
{
	public class EngineData : BaseData, IEngineData
	{
		public string StyleId { get; }

		public bool IsElectric
		{
			get {
				dynamic engines = JsonConvert.DeserializeObject<dynamic>(_response).engines;
				string electricFueltype = FuelTypeEnum.Electric.GetDisplayName();

				foreach (dynamic engine in engines)
					if (string.Equals(electricFueltype, engine.fuelType.ToString(), StringComparison.OrdinalIgnoreCase))
						return true;

				return false;
			}
		}

		public EngineData(string response, string styleId) : base(response)
		{
			StyleId = styleId;
		}
	}
}
