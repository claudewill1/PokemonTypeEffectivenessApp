using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PokemonTypeEffectiveness.Core.Models
{
    // respresents the type response from /type/{name} endpoint of PokeAPI
    public class TypeResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("damage_relations")]
        public DamageRelations DamageRelations { get; set; } = new();

    }

}