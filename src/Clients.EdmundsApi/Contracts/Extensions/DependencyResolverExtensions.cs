using System;
using Clients.EdmundsApi.Query;
using Clients.EdmundsApi.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Clients.EdmundsApi.Contracts.Extensions
{
	public static class DependencyResolverExtensions
    {
        public static IServiceCollection RegisterEdmundsApi(this IServiceCollection container, Func<IServiceProvider, IConfiguration> configurationBuilder)
        {
			{ // Singleton Http
				container.AddSingleton<IHttpClient, EdmundsHttpClient>();
			}

			{ // Register configuration provider.
				container.AddSingleton(configurationBuilder);
			}

			{ // Api methods
				container.AddTransient<IEngineQuery, EngineQuery>();
				container.AddTransient<IMakeQuery, MakeQuery>();
				container.AddTransient<IModelQuery, ModelQuery>();
				container.AddTransient<IStyleQuery, StyleQuery>();
			}

			return container;
        }
    }
}
