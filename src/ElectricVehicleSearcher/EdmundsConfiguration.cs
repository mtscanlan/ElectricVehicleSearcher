using System;
using Clients.EdmundsApi.Contracts.Extensions;
using Clients.EdmundsApi.Resources;

namespace ElectricVehicleSearcher
{
	public class EdmundsConfiguration : IConfiguration
	{
		public string ApiKey { get; }

		public EdmundsConfiguration(string apiKey)
		{
			if (string.IsNullOrEmpty(apiKey))
				throw new ArgumentNullException(nameof(apiKey));
			if (apiKey.Length != 24)
				throw new ArgumentException(
					ExceptionMessages.ApiKeyLengthArgumentException,
					nameof(apiKey));

			ApiKey = apiKey;
		}
	}
}
