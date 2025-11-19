using System.Threading.Tasks;
using PokemonTypeEffectiveness.Core.Models;

namespace PokemonTypeEffectiveness.Core.Services
{
    // Service interface to compute strengths and weaknesses using data from PokeAPI
    public interface IPokemonEffectivenessService
    {
        // Given a pokemon name, fetches its types and computes the types it is strong and weak against
        Task<PokemonTypeEffectivenessResult?> GetEffectivenessAsync(string pokemonName);
    }
}