using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonTypeEffectiveness.Code.Models;

namespace PokemonTypeEffectieness.Core.Services
{
    // The core logic required to convert the relations of type into strengths and weaknesses.
    public class PokemonTypeEffectivenessService : IPokemonEffectivenessService
    {
        private readonly IPokeApiClient _pokeApiClient;

        public PokemonTypeEffectivenessService(iPokeApiClient pokeApiClient)
        {
            _pokeApiClient = pokeApiClient;
        }

        public async Task<PokemonTypeEffectivenessResult?> GetEffectivenessAsync(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                throw new ArgumentException("Pokemon name cannot be empty.", nameof(pokemonName));
            }

            // Lookup the pokemon by name
            var pokemon = await _pokeApiClient.GetPokemonByNameAsync(pokemonName);

            if (pokemon == null)
            {
                throw new ApplicationException($"Pokemon\"{pokemonName}\" not found in PokeAPI.");
            }

            // collect the types the pokemon has
            var pokemonTypes = pokemon.Types
                .OrderBy(t => t.Slot)
                .Select(t => t.Type.Name)
                .ToList();

            if (pokemonTypes.Count == 0)
            {
                throw new ApplicationException($"Pokemon \"{pokemon.Name}\" has no types in the API response.");
            }

            // to avoid duplicates across multiple types, use hashsets
            var strong = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var weak = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // retrieve the damage relations for each type and incorporate them
            foreach (var typeName in pokemonTypes)
            {
                var typeDetails = await _pokeApiClient.GetTypeByNameAsync(typeName);
                if (typeDetails == null)
                {
                    // if type record not found, skip it
                    continue;
                }
                var relations = typeDetails.DamageRelations;

                // strong conditions from assinment
                // 1. does double damage to other types
                foreach (var t in relations.DoubleDamageTo)
                {
                    strong.Add(t.Name);
                }

                // 2. takes no damage from other types
                foreach (var t in relations.NoDamageFrom)
                {
                    strong.Add(t.Name);
                }

                // 3. takes half damage from other types
                foreach (var t in relations.HalfDamageFrom)
                {
                    strong.Add(t.Name);
                }

                // The assignment of weak conditions
                // 1. does no damage to other type
                foreach (var t in relations.NoDamageTo)
                {
                    weak.Add(t.Name);
                }
                // 2. does half damage to other types
                foreach (var t in relations.HalfDamageTo)
                {
                    weak.Add(t.Name);
                }
                // 3. takes double damage from other types
                foreach (var t in relations.DoubleDamageFrom)
                {
                    weak.Add(t.Name);
                }
            }
            // Generate stable sorted list for display
            var strongList = strong
                .Except(pokemonTypes, StringComparer.OrdinalIgnoreCase)
                // optionally exclude the pokemon's own types from strengths
                .OrderBy(t => t)
                .ToList();
            
            var weakList = weak
                .Except(pokemonTypes, StringComparer.OrdinalIgnoreCase)
                .OrderBy(t => t)
                .ToList();

            // return the result
            return new PokemonTypeEffectivnessResult
            {
                PokemonName = pokemon.Name,
                PokemonTypes = pokemonTypes,
                StrongAgainstTypes = strongList,
                WeakAgainstTypes = weakList 
            };
        }
    }
}