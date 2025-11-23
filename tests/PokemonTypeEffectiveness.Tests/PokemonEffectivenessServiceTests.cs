using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using PokemonTypeEffectiveness.Core.Models;
using PokemonTypeEffectiveness.Core.Services;
using Xunit;

namespace PokemonTypeEffectiveness.Tests
{
    public class PokemonEffectivenessServiceTests
    {
        [Fact]
        public async Task GetEffectivenessAsync_CombinesTypeRelationsCorrectly()
        {
            // Arrange
            var mockApiClient = new Mock<IPokeApiClient>();

            // Fake Pokemon that has a single type "water"
            mockApiClient.Setup(c => c.GetPokemonByNameAsync("squirtle"))
                .ReturnsAsync(new PokemonResponse
                {
                    Name = "squirtle",
                    Types = new List<PokemonTypeSlot>
                    {
                        new PokemonTypeSlot
                        {
                            Slot = 1,
                            Type = new NamedApiResource { Name = "water" }
                        }
                    }
                });

            // Fake type relations for "water" type
            mockApiClient.Setup(c => c.GetTypeByNameAsync("water"))
                .ReturnsAsync(new TypeResponse
                {
                    Name = "water",
                    DamageRelations = new DamageRelations
                    {
                        DoubleDamageTo = new List<NamedApiResource>
                        {
                            new NamedApiResource { Name = "fire"},
                            new NamedApiResource { Name = "ground" },
                            new NamedApiResource { Name = "rock" }
                        },
                        HalfDamageTo = new List<NamedApiResource>
                        {
                            new NamedApiResource { Name = "water" },
                            new NamedApiResource { Name = "grass" },
                            new NamedApiResource { Name = "dragon" }
                        },
                        NoDamageTo = new List<NamedApiResource>(),
                        DoubleDamageFrom = new List<NamedApiResource>
                        {
                            new NamedApiResource { Name = "electric" },
                            new NamedApiResource { Name = "grass" }
                        },
                        HalfDamageFrom = new List<NamedApiResource>
                        {
                            new NamedApiResource { Name = "fire" },
                            new NamedApiResource { Name = "water" },
                            new NamedApiResource { Name = "ice" },
                            new NamedApiResource { Name = "steel" }
                        },
                        NoDamageFrom = new List<NamedApiResource>()
                    }
                });

            var service = new PokemonTypeEffectivenessService(mockApiClient.Object);

            // Act
            var result = await service.GetEffectivenessAsync("squirtle");

            // Assert
            Assert.Equal("squirtle", result.PokemonName);
            Assert.Contains("water", result.PokemonTypes);

            // Strong against types double damage to fire, ground, rock
            Assert.Contains("fire", result.StrongAgainstTypes);
            Assert.Contains("ground", result.StrongAgainstTypes);
            Assert.Contains("rock", result.StrongAgainstTypes);

            // Weak against types double damage from electric and grass
            Assert.Contains("electric", result.WeakAgainstTypes);
            Assert.Contains("grass", result.WeakAgainstTypes);
        }

        [Fact]
        public async Task GetEffectivenessAsync_InvalidPokemon_ThrowsApplicationException()
        {
            // Arrange
            var mockApiClient = new Mock<IPokeApiClient>();

            mockApiClient.Setup(c => c.GetPokemonByNameAsync("nosuchpokemon"))
                .ReturnsAsync((PokemonResponse?)null);

            var service = new PokemonTypeEffectivenessService(mockApiClient.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                await service.GetEffectivenessAsync("nosuchpokemon");
            });
        }
    }
}
