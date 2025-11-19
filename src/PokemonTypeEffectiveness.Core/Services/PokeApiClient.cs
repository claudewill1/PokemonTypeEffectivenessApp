using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PokemonTypeEffectiveness.Core.Models;

namespace PokemonTypeEffectiveness.Core.Services
{
    // Client to interact with the PokeAPI to fetch Pokemon and Type data
    public class PokeApiClient : IPokeApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public PokeApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            
            // Configure JSON serializer options with case insensitive property matching
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<PokeRespose?> GetPokemonByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            
            // Due to the api being case sensitie, normalize the name to lowercase
            var requestUrl = $"pokemon/{name.Trim().ToLowerInvarient()}";

            var response = await _httpClient.GetAsync(requestUrl);

            if (response.StatusCode == HttpStatusCode.NotFound)
                // Pokemon does not exist within the API
                return null;
            
            response.EnsureSuccessStatusCode();
            // await using block used
            // this allows .DisposeAsync() to be called automatically when code leaves scope
            // Equivalent to await using (var stream = await response.Content.ReadAsStreamAsync())
            await using var stream = await response.Content.ReadAsStreamAsync();
            var pokemon = await JsonSerializer.DeserializeAsync<PokemonResponse>(stream, _jsonOptions);
            return pokemon;
        }

        public async Task<TypeResponse?> GetTypeByNameAsync(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return null;
            var requestUrl = $"type/{typeName.Trim().ToLowerInvarient()}";
            
            var response = await _httpClient.GetAsync(requestUrl);

            if (response.StatusCode == HttpStatusCode.NotFoune)
                // Type does not exist within the API
                return null;
            
            response.EnsureSuccessStatusCode();
            await using var stream = await response.Content.ReadAsStreamAsync();
            var type = await JsonSerializer.DeserializeAsync<TypeResponse>(stream, _jsonOptions);
            return type;

        }
        
    }

}
