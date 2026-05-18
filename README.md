# Project Nahual
Project Nahual is a first person dungeon crawler shooter with procedural generation where the player can choose between three animal classes to survive against relentless forest hunters. 
This project was started during the [GGJ 2026](https://globalgamejam.org/games/2026/spirit-nahual-7), using the theme *"Masks"* and implements **MVC**, **Cline** usage for simpler tasks and a **CI/CD workflow** for automated builds.

## Overview
The project is currently developed in Unity 6000.3.6f1 since its stability in some required packages such as Probuilder and WebGL support. Project structure is divided in modules separated by a namespace and assemblies.

## Game Loop
This a core module that handles game logic and initializes the **FP Character** and **PCG modules**. `Game` is used to handle start, finish level and game over events to initialize `LevelGenerator` and `ClassProfileSelector`, and `ClassProfileSelector` controls different class profiles and initializes the `FPCharacter` class.

## PCG
This is a core module that handles asset generation in grid-based procedural worlds. `LevelGenerator` is the main class of this module and generates a logic class `PCGAlgorithm` that handles positioning of grid and border collisions, and supports object placement for environment, enemies and level gateways. `LevelGenerator` also positions player and the ground plane and bakes the `NavMeshSurface` before placing enemies.

This module also has a `Test` submodule to validate the `PCGAlgorithm`, as well as some helper classes that deal with object pooling (`PCGAssetLibrary`) and randomize objects when instanced (`PCGAsset`). Basic functionality of this module is inspired by the [A Beginners Guide To Procedural Generation](https://www.youtube.com/playlist?list=PLu2uAkIZ4shpPdCTIjEpvhD8U-RRM3Y2F) series.

## First Person Character
This module initializes core functionality for player and implements the **Player Input** and **Weapons** modules. `FPCharacter` is the main class and passes an `IPlayerInput` interface to handle the events and inputs for the `MovementController`, `CameraController` and `WeaponController` classes. 

## Player Input
This module uses a generic `IPlayerInput` interface that is used by the `StandaloneInputController`. It's important to mention that this project uses the modern **Unity Input Package**, but inputs can be rewritten with the legacy input system if you want by creating another `IPlayerInput`.

## Weapons
This module uses a generic `IWeapon` interface that is passed by the `WeaponController` in the **FPCharacter** module. The `WeaponBase` class is used as parent to give basic functionality to the other weapon classes, while `WeaponLogic` is used to handle damage to `IDamageable` instances. This module also has a **Test** submodule that validates weapon logic implementation.

NOTE: The **Enemy** module has a loose implementation of a `WeaponBase` class in the `Axe` class, but it is going to be refactored into an `IWeapon` class to simplify logic and coherence with the codebase. 

## FSM
This module features a generic `FiniteStateMachine` class that implements `IState` interfaces used by the **Enemy** module. The `BaseBehaviour` class handles the `FiniteStateMachine` initialization and resetting, and basic functionality for the `NavMeshAgent` class, such as initializing position, enabling or disabling the agent. 

WARNING: **Nav Mesh** limitations may cause agents to crash between them at times, this will be worked in a latter commit.

## Enemy
This module extends the **FSM** module and implements `BaseBehaviour` child classes for specific enemy types. The `Enemy` base class handles damage events and initializes the logic of the `BaseBehaviour` class.

## Utils
This module has classes that handle:
* Object registering to avoid GameObject.Find cost usage - `Registry`
* Animation events called from the Animation window - `AnimationContext`
* Locking cursor or enabling it for UI navigation - `CursorHandler`