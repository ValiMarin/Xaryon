# Xaryon

A first-person shooter developed in Unity.

## Playable Build

[Download the Windows prototype](https://github.com/ValiMarin/Xaryon/releases/tag/v0.1.0)

### Installation

1. Download `Xaryon.rar` from the release's **Assets** section.
2. Extract the entire archive.
3. Open the extracted folder and launch `Turretification.exe`.

Keep the executable and its accompanying files together.

## Controls

| Input | Action |
|---|---|
| W / A / S / D | Move |
| Mouse | Look around |
| Left Shift | Sprint |
| Space | Jump |
| Left Mouse Button | Attack / Shoot |
| Right Mouse Button | Block |
| Mouse Wheel | Switch weapons |
| 1 | Equip melee weapon |
| 2 | Equip pistol |
| E | Use melee skill / Change firing mode, depending on the equipped weapon |
| R | Reload |
| Q | Use player ability |
| F | Interact |

## About

Xaryon is a personal first-person shooter project developed in Unity. All gameplay code was written by me.

This repository includes playable builds and selected source code showcasing the game's systems.

## My Contributions

Xaryon is a solo-developed project. I created the following from scratch:

- All gameplay code and systems.
- All 3D models, rigging, and animations.
- All textures.
- Original sound effects.

The game's audio combines my own sound effects with selected third-party sounds sourced online.

## Source Code

Selected gameplay systems from Xaryon, with documentation explaining their behaviour, component responsibilities, and design decisions.

### Turret System

Shared turret behaviour with specialized Classic, Sniper, Bomber, and Penetrator variants.

- Inheritance and polymorphism for specialized enemy behaviour.
- Separate components for detection, shooting, animation, and feedback.
- Interfaces for damage handling and highlighting.
- ScriptableObjects for configuration.

[Explore the turret system](Source/TurretSystem/README.md)

### Player Combat

A weapon system combining a firearm with multiple firing modes and a sword with combos and an ability.

- Interface-based weapon management and animation-event dispatch.
- Separate components for attacks, recoil, ammunition, and feedback.
- Animation-driven attack timing, reload completion, and combo windows.
- Configurable weapon and ability settings.

[Explore player combat](Source/PlayerCombat/README.md)

### Loot and Pickup UI

World pickups connected to on-screen collection notifications.

- Configurable loot drops and pickup rewards.
- Grouping of repeated pickups by type.
- Reusable notification slots with individual display timers.
- Separation between collection logic and UI presentation.

[Explore loot and pickup UI](Source/LootAndPickupUI/README.md)

These selections are provided for code review. They require the full Unity project and its assets to run. To play the game, download the build linked above.

## Tools & Technologies

| Area | Tools |
|---|---|
| Game Engine | Unity, Universal Render Pipeline (URP) |
| Programming | C#, Visual Studio |
| 3D Modeling & Animation | Autodesk Maya |
| Texture Editing | Adobe Photoshop |
| Audio Editing | Audacity |
| Version Control & Hosting | Git, GitHub |

## Project Status

The current release is an early playable prototype. It does not represent the final game.
