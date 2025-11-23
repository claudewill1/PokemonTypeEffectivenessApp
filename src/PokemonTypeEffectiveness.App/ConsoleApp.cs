using System;
using System.Threading.Tasks;
using PokemonTypeEffectiveness.Core.Models;
using PokemonTypeEffectiveness.Core.Services;

namespace PokemonTypeEffectiveness.App
{
    // The user interaction for the console application is handled here.
    public class ConsoleApp
    {
        private readonly IPokemonTypeEffectivenessService _effectivenessService;

        public ConsoleApp(IPokemonTypeEffectivenessService effectivenessService)
        {
            _effectivenessService = effectivenessService;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Pokemon Type Effectiveness Tool");
            Console.WriteLine("Enter a Pokemon name to see its strengths and weaknesses.");
            Console.WriteLine("Type \"exit\" to quit.");

            Console.WriteLine();
            while (true)
            {
                System.Console.Write("Enter the name of a Pokemon: ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Please enter a non empty/valid Pokemon name.");
                    continue;
                }

                if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting the application. Goodbye!");
                    break;
                }

                try
                {
                    var result = await _effectivenessService.GetEffectivenessAsync(input.Trim());
                    PrintResult(result);
                }
                catch (ApplicationException ex)
                {
                    // Application level exception, Pokemon not found or missing type data
                    Console.WriteLine($"Problem: {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    // Argument exception, invalid input
                    Console.WriteLine($"Input Error: {ex.Message}");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("There was a problem while trying to reach the PokeAPI. Please check your connection and try again");                
                    Console.WriteLine($"Details: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Unexpected exception
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
        }

        // Display the effectiveness result in the console
        private static void PrintResult(PokemonTypeEffectivenessResult result)
        {
            Console.WriteLine($"\nResults for Pokemon: {result.PokemonName}");
            Console.WriteLine("Type Effectiveness:");
        
            foreach (var type in result.PokemonTypes)
            {
                Console.WriteLine($"- {type}"); 
            }

            Console.WriteLine();

            if (result.StrongAgainstTypes.Count == 0)
            {
                Console.WriteLine("This Pokemon has no types it is strong against."); 
            }
            else
            {
                Console.WriteLine("Strong Against:");
                foreach (var t in result.StrongAgainstTypes)
                {
                    Console.WriteLine($"- {t}");
                }
            }
            Console.WriteLine();
            if (result.WeakAgainstTypes.Count == 0)
            {
                Console.WriteLine("This Pokemon has no types it is weak against.");
            }
            else
            {
                Console.WriteLine("Weak Against:");
                foreach (var t in result.WeakAgainstTypes)
                {
                    Console.WriteLine($"- {t}");
                }
            }
            
        }
    }
}