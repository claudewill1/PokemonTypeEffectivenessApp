using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PokemonTypeEffectiveness.Core.Models
{
    // Respresents the Pokemon response from the PokeAPI
    public class PokemonResponse
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        // A list of type slots for the Pokemon
        [JsonPropertyName("types")]
        public List<PokemonTypeSlot> Types { get; set; } = new();

    }

    
}