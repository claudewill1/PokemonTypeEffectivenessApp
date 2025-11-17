using System.Threading.Tasks;
using PokemonTypeEffectiveness.Core.Models;

namespace PokemonTypeEffectiveness.Core.Services
{
    // Interface for PokeApiClient to fetch Pokemon and Type data
    public interface IPokeApiClient
    {
        // Fetches Pokemon data by name from the PokeAPI, returns details for pokemon if found
        // otherwise returns null
        Task<PokemonResponse?> GetPokemonByNameAsync(string name);

        // Fetches Type data by name from the PokeAPI, returns details for type if found
        Task<TypeResponse?> GetTypeByNameAsync(string typeName);
    }
}