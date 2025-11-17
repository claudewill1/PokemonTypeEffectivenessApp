using System.Collections.Generic;

namespace PokemonTypeEffectiveness.Core.Models
{
    // the result object returned and shown to the user
    public class PokemonTypeEffectivenessResult
    {
        // The name of the Pokemon returned by the API
        public string Pokename { get; set; } = string.Empty;

        // The types that the pokemon has (e.g. fire, flying)
        public List<string> Types { get; set; } = new();

        // List of types that this pokemon is strong against
        public List<string> StrongAgainstTypes { get; set; } = new();

        // List of types that this pokemon is weak against
        public List<string> WeakAgainstTypes { get; set; } = new();

    }
}