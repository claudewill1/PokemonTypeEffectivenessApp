using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PokemonTypeEffectiveness.Core.Services;

namespace PokemonTypeEffectiveness.Console
{
    // Since the class is not intended for public consumption, keeping it internal 
    // is cleaner and follows encapsulation principles.
    // This also helps to avoid unnecessary exposure of implementation details.
    internal class Program
    {
        // Entry point. Uses async so can await API calls cleanly
        public static async Task Main(string[] args)
        {
            // Setup dependency injection
            var services = new ServiceCollection();

            // Register HttpClient and PokemonApiClient
            services.AddHttpClient<IPokemonApiClient, PokemonApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
            });

            // register core effectiveness service
            services.AddTransient<IPokemonTypeEffectivenessService, PokemonTypeEffectivenessService>();

            // register console coordinator class
            services.AddTransient<ConsoleApp>();

            var serviceProvider = services.BuildServiceProvider();

            // Resolve ConsoleApp and start the app
            var app = serviceProvider.GetRequiredService<ConsoleApp>();
            await app.RunAsync();

        }
    }
}
