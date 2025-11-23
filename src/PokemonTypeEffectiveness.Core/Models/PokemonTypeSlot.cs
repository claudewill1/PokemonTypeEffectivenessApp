using System.Text.Json.Serialization;
namespace PokemonTypeEffectiveness.Core.Models
{
    // Represents the type information for a Pokemon
    public class PokemonTypeSlot
    {
        [JsonPropertyName("slot")]
        public int Slot { get; set; }

        [JsonPropertyName("type")]
        public NamedApiResource type { get; set; } = new();
    }
}
   