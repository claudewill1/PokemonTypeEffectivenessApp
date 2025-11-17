using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PokemonTypeEffectiveness.Core.Models
{
    // Respresents the Pokemon response from the PokeAPI
    public class PokemonResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        // A list of type slots for the Pokemon
        [JsonPropertyName("types")]
        public List<PokemonTypeSlot> Types { get; set; } = new();

    }

    // Represents the type information for a Pokemon
    public class PokemonTypeInfo
    {
        [JsonPropertyName("slot")]
        public int Slot { get; set; }

        [JsonPropertyName("type")]
        public NameApiResource type { get; set; } = new();
    }

    // small generic resource with the name and url which are used by PokeAPI
    public class NameApiResource
    {
        [JsonPropertyName("name")]
        public string name { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string url { get; set; } = string.Empty;
    }
}