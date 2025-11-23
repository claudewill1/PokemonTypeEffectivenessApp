using System.Text.Json.Serialization;
namespace PokemonTypeEffectiveness.Core.Models
{
    // small generic resource with the name and url which are used by PokeAPI
    public class NamedApiResource
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}