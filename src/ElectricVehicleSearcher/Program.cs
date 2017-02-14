using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Clients.EdmundsApi.Contracts;
using Clients.EdmundsApi.Contracts.Extensions;
using Clients.EdmundsApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ElectricVehicleSearcher
{
	/// <remarks>
	/// 1. Please note the DEBUG directives to switch between release and debug within the helpers region. The API 
	/// key given to me from Edmunds only allows for 25 calls per day. Due to this limitation, I have created four 
	/// helper methods which conditionally load mock data depending on if the application is run in DEBUG or RELEASE 
	/// mode. I have decided to leave these in for my submission to demonstrate my development process. Furthermore, 
	/// I have been unable to run this project to completion using only API calls due to this limitation.
	/// 
	/// 2. Due to this being an "internal" tool with a very specific purpose (and a code challenge, and for 
	/// simplicity's sake), I have made the design decision to use a very focused wrapper to the Edmunds API utilizing 
	/// dynamics rather than creating serializable models. I have also incorporated helper methods into the four 
	/// objects MakeData, ModelData, EngineData and StyleData to return custom data ie. IsElectric and AllMakes, 
	/// rather than exposing a full blown wrapper to the Edmunds APIs.
	/// </remarks>
	internal class Program
	{
		private static IConfigurationRoot _configuration;
		private static ILogger<Program> _logger;
		private static IServiceProvider _serviceProvider;

		public static void Main(string[] args)
		{
			try
			{
				// Initialize startup.
				_configuration = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json")
					.Build();

				_serviceProvider = new ServiceCollection()
					.AddLogging()
					.RegisterEdmundsApi((x) => new EdmundsConfiguration(_configuration["ApiKey"]))
					.BuildServiceProvider();

				_logger =
					_serviceProvider
					.GetRequiredService<ILoggerFactory>()
#if DEBUG
					.AddDebug(LogLevel.Debug)
#else
					.AddConsole(LogLevel.Information)
#endif
					.CreateLogger<Program>();

				_logger.LogDebug($"{nameof(IConfigurationRoot)} initialized [{ _configuration != null}]");
				_logger.LogDebug($"{nameof(IServiceProvider)} initialized [{ _serviceProvider != null}]");
				_logger.LogDebug($"{nameof(ILogger<Program>)} initialized [{ _logger.IsEnabled(LogLevel.Information)}]");

				// Query all makes.
				IMakeData makeData = GetMakeDataAsync().Result;

				// Query all models by providing all makes through the GetModelDataAsync method.
				_logger.LogInformation($"Searching through {makeData.AllMakes.Count()} makes of vehicles, this will take a moment.");
				Task<IModelData>[] awaitableModelData = makeData.AllMakes.Select(s => GetModelDataAsync(s)).ToArray();
				Task.WaitAll(awaitableModelData);

				// Select all StyleIds from the result of the model query for all submodels of every make and model
				IEnumerable<string> styleIds = awaitableModelData.SelectMany(s => s.Result.StyleIds);
				_logger.LogDebug($"Retreived {styleIds.Count()} {nameof(styleIds)} for all submodels of every make and model.");

				// Query all engine data for all submodels of every make and model (ie Tesla Model S 60 and Tesla Model S P100D).
				Task<IEngineData>[] awaitableEngineData = styleIds.Select(s => GetEngineDataAsync(s)).ToArray();
				Task.WaitAll(awaitableEngineData);

				// Select all the StyleIds for vehicles that have Electric motors as their standard equipment.
				IEnumerable<string> electricVehicleStyleIds = awaitableEngineData.Where(w => w.Result.IsElectric).Select(s => s.Result.StyleId);
				_logger.LogDebug($"Retrieved {electricVehicleStyleIds.Count()} {nameof(electricVehicleStyleIds)}, getting full details.");

				// Query full data for a given style by providing the StyleIds for electric vehicles only.
				Task<IStyleData>[] awaitableStyleData = electricVehicleStyleIds.Select(s => GetStyleDataAsync(s)).ToArray();
				Task.WaitAll(awaitableStyleData);

				// Iterate over the full data set and print out the information for the vehicle.
				foreach (var styleData in awaitableStyleData)
				{
					// We have the string we want to print in the string object "style" defined below, 
					// I will continue doing what the old application was doing and print only the first 
					// 200 characters to the console window, though it would probably make more sense to
					// print the whole JSON object or pipe it to a file.
					string style = styleData.Result.ToString();
					_logger.LogInformation(style.Substring(0, 200));
				}

				_logger.LogInformation($"Search complete, found {electricVehicleStyleIds.Count()} new electric vehicle(s).");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			finally
			{
				Console.Read();
			}
		}

		#region Development Helpers
		private static Task<IMakeData> GetMakeDataAsync()
		{
#if DEBUG
			return Task.Run<IMakeData>(() =>
			{
				var testData = File.ReadAllText("TestData/Makes.json");
				return new MakeData(testData);
			});
#else
			var makeQuery = _serviceProvider.GetService<IMakeQuery>();
			return makeQuery.GetAllMakesAsync(DateTime.Now.Year, StateEnum.New, ViewEnum.Basic);
#endif
		}

		private static Task<IModelData> GetModelDataAsync(string make)
		{
#if DEBUG
			return Task.Run<IModelData>(() =>
			{
				var testData = File.ReadAllText("TestData/ModelsChevrolet.json");
				return new ModelData(testData);
			});
#else
			var modelQuery = _serviceProvider.GetService<IModelQuery>();
			return modelQuery.GetAllModelsAsync(make, DateTime.Now.Year, StateEnum.New, ViewEnum.Basic);
#endif
		}

		private static Task<IEngineData> GetEngineDataAsync(string styleId)
		{
#if DEBUG
			return Task.Run<IEngineData>(() =>
			{
				var testData = File.ReadAllText("TestData/EnginesBolt.json");
				return new EngineData(testData, styleId);
			});
#else
			var engineQuery = _serviceProvider.GetService<IEngineQuery>();
			return engineQuery.GetEnginesAsync(styleId, AvailabilityEnum.Standard);
#endif
		}

		private static Task<IStyleData> GetStyleDataAsync(string styleId)
		{
#if DEBUG
			return Task.Run<IStyleData>(() =>
			{
				var testData = File.ReadAllText("TestData/StylesBolt.json");
				return new StyleData(testData);
			});
#else
			var styleQuery = _serviceProvider.GetService<IStyleQuery>();
			return styleQuery.GetStyleDetailsAsync(styleId, ViewEnum.Full);
#endif
		}
		#endregion
	}
}
