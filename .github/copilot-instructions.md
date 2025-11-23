<!--
Guidance for AI coding agents working on the PokemonTypeEffectiveness repo.
Focus: short, actionable, and specific to this codebase.
-->

# Copilot / Agent Instructions — PokemonTypeEffectiveness

Purpose: give an AI agent the immediate, concrete knowledge to be productive in this repo: where business logic lives, how services communicate, important conventions, and typical developer commands.

- Keep this file concise. Prefer actionable rules and file references over generic advice.

## Big picture
- Solution contains two main runtime projects under `src/`:
  - `PokemonTypeEffectiveness.Core` — domain and service layer (models, PokeApi client, effectiveness service).
  - `PokemonTypeEffectiveness.Console` — minimal console runner wiring DI and launching the `ConsoleApp`.
- Flow: ConsoleApp -> IPokemonTypeEffectivenessService -> IPokeApiClient -> calls to external PokeAPI (https://pokeapi.co/api/v2/).

## Key files to read first
- `src/PokemonTypeEffectiveness.Console/Program.cs` — DI registration and HttpClient BaseAddress.
- `src/PokemonTypeEffectiveness.Core/Services/PokeApiClient.cs` — HTTP calls, JSON (case-insensitive) deserialization, and error handling.
- `src/PokemonTypeEffectiveness.Core/Services/PokemonTypeEffectivenessService.cs` — core algorithm to merge damage relations into "strong/weak" lists (primary logic to test/extend).
- `src/PokemonTypeEffectiveness.Core/Models/` — DTOs returned by PokeAPI and result objects (inspect when fixing deserialization or tests).
- `tests/PokemonTypeEffectiveness.Tests/PokemonEffectivenessServiceTests.cs` — unit-test patterns (uses xUnit and Moq). Tests show expected service behavior and example mock setups.

## Project-specific conventions & patterns
- Dependency Injection: services are registered in `Program.cs` using Microsoft DI. Use constructor injection in classes.
- HttpClient factory: `AddHttpClient<IPokemonApiClient, PokemonApiClient>` with `BaseAddress` set to `https://pokeapi.co/api/v2/`.
- JSON handling: `PokeApiClient` uses System.Text.Json with `PropertyNameCaseInsensitive = true`.
- Error handling and return conventions in this codebase:
  - API not-found typically returns `null` from client methods (e.g., `GetPokemonByNameAsync` / `GetTypeByNameAsync`).
  - The service throws `ApplicationException` in several failure scenarios (tests expect this behavior).
- Testing: unit tests mock the API client (Moq) and assert on resulting `StrongAgainstTypes` / `WeakAgainstTypes` lists. Follow the same mocking style when adding tests.

## Commands developers use (PowerShell / Windows)
- Build solution:
```powershell
dotnet build PokemonTypeEffectiveness.sln
```
- Run console app:
```powershell
dotnet run --project src/PokemonTypeEffectiveness.Console
```
- Run tests:
```powershell
dotnet test tests/PokemonTypeEffectiveness.Tests
```

## Integration points & external dependencies
- External API: PokéAPI (https://pokeapi.co/api/v2/). Keep calls resilient to 404s and transient failures.
- Test dependencies: xUnit, Moq. See `tests/` project file for versions.

## Things to watch for (observed code quirks)
These are real, discoverable issues an agent must not silently 'fix' without running tests or CI — sometimes tests expect current behavior.
- Typo and naming mismatches exist in the source (examples seen while scanning):
  - `ToLowerInvarient()` spelled incorrectly (should be `ToLowerInvariant()`) in `PokeApiClient` request URL normalization.
  - `HttpStatusCode.NotFoune` / `NotFoune` typos and other misspellings appear in checks.
  - Several type and namespace name mismatches between interfaces and implementations (e.g., `IPokemonApiClient` vs `IPokeApiClient`) — check the exact symbol before renaming.
  - `PokemonTypeEffectivenessService` contains unfinished local variables (e.g., `weakList`, `strongList` usage is incomplete) and commented/placeholder logic.

When making changes:
- Run the unit tests after edits. Many issues are small typos; run `dotnet test` to ensure behavior still matches tests.
- Prefer small, well-scoped PRs fixing one class or test at a time.
- If you rename public symbols or interfaces, update all references and run a build; the repository uses standard C# project files so the compiler will catch missing references.

## How to extend or fix the core logic
- To change or extend damage-calculation logic, modify `PokemonTypeEffectivenessService.cs` and update/add unit tests in `tests/...` to cover both happy path and at least one error condition (invalid pokemon -> ApplicationException).
- When adding HTTP behavior changes, update `PokeApiClient.cs`. Keep JSON deserialization options (`PropertyNameCaseInsensitive = true`) unless there's a specific reason.

## Example actionable tasks for an agent
- Fix typos in `PokeApiClient` (`ToLowerInvarient` -> `ToLowerInvariant`, `NotFoune` -> `NotFound`), then run tests.
- Complete `PokemonTypeEffectivenessService` implementation so it returns `PokemonTypeEffectivenessResult` with filled `StrongAgainstTypes` and `WeakAgainstTypes` lists; mirror expectations in `PokemonEffectivenessServiceTests.cs`.
- Add logging to `PokeApiClient` for non-success responses (optional small improvement).

## If something is unclear
- Open the files listed under "Key files" and run the tests locally. If test failures reference missing or renamed symbols, prefer to update the code to a passing, type-safe state and keep changes minimal.

---
If you'd like, I can now:
1. Create this file in the repo (I will) and
2. Run `dotnet build` and `dotnet test`, then report failures to guide next fixes.

Please tell me which of the follow-ups you'd like me to do next.
