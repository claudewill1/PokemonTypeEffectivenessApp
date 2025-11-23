# Pokémon Type Effectiveness Console App

This application is a .NET console program that connects to the public **PokéAPI** to determine a Pokémon’s strengths and weaknesses based on its types. Each Pokémon has one or more elemental types, and each type interacts differently with other types. For example, Fire is strong against Grass but weak against Water.

The goal of the app is to give users a simple tool where they can enter a Pokémon name and receive a clear summary of:

- Which types the Pokémon is **strong** against  
- Which types the Pokémon is **weak** against  
- What types the Pokémon **has**  

The console app retrieves real Pokémon data using the PokéAPI, processes the official damage-relation tables, and outputs a human-readable summary of strengths and weaknesses.

---

## **Purpose of the Application**

The purpose of this project is to:

- Demonstrate how to build a clean, well-structured .NET console application  
- Show how to consume external REST APIs (PokéAPI) in C#  
- Provide a practical example of dependency injection in a console environment  
- Make Pokémon type interactions easy to understand for users learning the type-effectiveness system  
- Include unit testing and clean architecture principles from the start  

This application is designed as a beginner-friendly and developer-friendly example of combining .NET, HTTP APIs, and modular service design.

---

## **How It Works (High Level)**

1. The user enters the name of a Pokémon (e.g., *pikachu*, *charizard*, *bulbasaur*).
2. The app retrieves the Pokémon’s type information from the PokéAPI.
3. For each type (electric, fire, water, etc.), the app loads the official damage-relation rules.
4. The app calculates which types the Pokémon is strong or weak against.
5. The results are printed in a simple, readable format.

---

## **Example Usage**

```
Pokémon Type Effectiveness Tool
Enter a Pokémon name:
pikachu

Results for pikachu
Types:
  - electric

Strong against:
  - flying
  - water

Weak against:
  - ground
```

---

## **Requirements**

- .NET 8 SDK or newer  
- Internet connection (required to call PokéAPI)

---

## **Running the App**

Once cloned:

```bash
dotnet run --project src/PokemonTypeEffectiveness.App
```

You will then be prompted to enter a Pokémon name.

---

## **Current Status**

This is the initial version of the README and project layout.  
Further sections such as architecture details, dependency injection explanations, unit testing notes, and API mapping will be added as the project progresses.
