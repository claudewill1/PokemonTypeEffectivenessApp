using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using PokemonTypeEffectiveness.Core.Models;
using PokemonTypeEffectiveness.Core.Services;

namespace PokemonTypeEffectieness.Core.Services
{
    // The core logic required to convert the relations of type into strengths and weaknesses.
    public class PokemonTypeEffectivenessService : IPokemonEffectivenessService
    {
        private readonly IPokeApiClient _pokeApiClient;

        public PokemonTypeEffectivenessService(IPokeApiClient pokeApiClient)
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
            
            // Create a list that will store the final "strong against types
            var strongList  = new List<string>();

            // Iterate through each type the Pokemon is strong against
            foreach (var type in strong)
            {
                // This flag is used to have it checked whether or not a type is one of the Pokemon's own types
                var isOwnType = false;

                // compare type against pokemon's own types
                foreach (var pokemonType in pokemonTypes)
                {
                    // If type matches a type belonging to the pokemon, mark it so it can be skipped.
                    if (string.Equals(type, pokemonType, StringComparison.OrdinalIgnoreCase))
                    {
                        isOwnType = true;
                        break; // stop checking for further types since a match was found
                    }
                }

                // Add type to the final strong list only if the type is not one of the pokemon's own types
                if (!isOwnType) strongList.Add(type);
            }
            // Sort the strong list alphabetically for clean output (case-insensitive)
            strongList.Sort(StringComparer.OrdinalIgnoreCase);

            // create list to store final "weak against" types
            var weakList = new List<string>();
            // Iterate through each type the Pokemon is strong against
            foreach (var type in strong)
            {
                // This flag is used to have it checked whether or not a type is one of the Pokemon's own types
                var isOwnType = false;

                // compare type against pokemon's own types
                foreach (var pokemonType in pokemonTypes)
                {
                    // If type matches a type belonging to the pokemon, mark it so it can be skipped.
                    if (string.Equals(type, pokemonType, StringComparison.OrdinalIgnoreCase))
                    {
                        isOwnType = true;
                        break; // stop checking for further types since a match was found
                    }
                }

                // Add type to the final weak list only if the type is not one of the pokemon's own types
                if (!isOwnType) weakList.Add(type);

                // sort final weak against list alphabetically for cleant output (case-insensitive) 
            }

            // 


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