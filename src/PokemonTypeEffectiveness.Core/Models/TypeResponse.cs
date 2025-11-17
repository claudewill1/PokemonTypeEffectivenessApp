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

    // All of the damage relations that describe how this type interacts with others contained here.
    public class DamageRelations
    {
        [JsonPropertyName("double_damage_from")]
        public List<NamedApiResource> DoubleDamageFrom { get; set; } = new();

        [JsonPropertyName("double_damage_to")]
        public List<NamedApiResource> DoubleDamageTo { get; set; } = new();

        [JsonPropertyName("half_damage_from")]
        public List<NamedApiResource> HalfDamageFrom { get; set; } = new();

        [JsonPropertyName("half_damage_to")]
        public List<NamedApiResource> HalfDamageTo { get; set; } = new();

        [JsonPropertyName("no_damage_from")]
        public List<NamedApiResource> NoDamageFrom { get; set; } = new();

        [JsonPropertyName("no_damage_to")]
        public List<NamedApiResource> NoDamageTo { get; set; } = new();

    }
}