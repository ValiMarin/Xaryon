# Player Combat

Selected source code from Xaryon, a first-person shooter developed in Unity.

This selection covers weapon switching, a firearm with multiple firing modes, and a sword with combo and ability behaviour.

## Start Here

- [WeaponManager](Weapons/WeaponManager.cs) — spawns the loadout, selects the active weapon, and coordinates weapon switching with animation.
- [RightHandAnimEventsHandler](Weapons/RightHandAnimEventsHandler.cs) — routes animation events to the current weapon through interfaces.
- [PistolController](Weapons/Pistol-Scripts/PistolController.cs) — manages firing modes, magazines, reloading, and weapon events.
- [SwordController](Weapons/Melee-Scripts/Swords-Scripts/SwordController.cs) — coordinates melee input, combo windows, and ability activation.

## Weapon Switching

The manager holds weapons through [IWeapon](Shared/IWeapon.cs). Selecting a weapon updates the reference used by the animation-event handler; the animation event then activates the selected weapon and hides the other one.

The animation-event handler calls `DealDamage()` through `IWeapon`. Additional actions are dispatched only when the weapon implements the corresponding interface:

| Interface | Responsibility |
|---|---|
| [IWeapon](Shared/IWeapon.cs) | Apply the weapon's attack |
| [IReloadable](Shared/IReloadable.cs) | Complete a reload |
| [IMeleeWeapon](Shared/IMeleeWeapon.cs) | Start melee feedback and activate a melee ability |
| [IComboWeapon](Shared/IComboWeapon.cs) | Open and close combo input windows |

This demonstrates polymorphism through interfaces: the same animation-event entry point can dispatch to different weapon implementations.

## Firearm Flow

1. Input requests firing, reloading, or a mode switch.
2. The controller checks ammunition, fire timing, and reload or sprint restrictions.
3. Weapon events drive the animation and feedback components.
4. A damage animation event reaches the controller through the shared animation-event handler.
5. [PistolShooting](Weapons/Pistol-Scripts/PistolShooting.cs) performs the raycast or spawns a projectile, applies feedback, and reports the fired round.
6. A reload completion event transfers ammunition from [PlayerAmmo](Shared/PlayerAmmo.cs) into the current magazine.

The available modes and their settings come from [PistolSO](Weapons/Pistol-Scripts/PistolSO.cs). Magazine ammunition is tracked per mode, while `PlayerAmmo` holds reserve ammunition.

[PistolRecoil](Weapons/Pistol-Scripts/PistolRecoil.cs) exposes a recoil offset consumed by the camera system.

## Melee Flow

The sword separates attack coordination from damage, ability resources, dash movement, and audiovisual feedback:

- [SwordAttack](Weapons/Melee-Scripts/Swords-Scripts/SwordAttack.cs) handles basic attack damage and feedback.
- `SwordController` manages attack input and combo-window flags updated by animation events.
- [SwordDash](Weapons/Melee-Scripts/Swords-Scripts/SwordDash.cs) selects a damageable target and moves the player toward it.
- [SwordAbility](Weapons/Melee-Scripts/Swords-Scripts/SwordAbility.cs) manages ability targets, cooldown, and energy cost.
- [SwordStatusSO](Weapons/Melee-Scripts/Swords-Scripts/SwordStatusSO.cs) supplies damage, range, and ability settings.

The ability consumes energy through [ISuperEnergyUser](Shared/ISuperEnergyUser.cs), rather than requiring a particular energy-provider class.

## Design

- Private runtime state with focused public methods and events.
- Interface-based dispatch for shared weapon actions and optional capabilities.
- Separate components for weapon coordination, damage, recoil, animation, audio, and visual effects.
- Animation events for attack timing, combo windows, switching, and reload completion.
- ScriptableObjects for weapon and projectile configuration.
- The [ShootingMode](Shared/ShootingMode.cs) enum identifies firearm modes.

## Scope and Dependencies

This is a source-code showcase extracted from the game, not a standalone Unity project.

The selection includes the right-hand weapon scripts and supporting interfaces, ammunition storage, and the firing-mode enum. It also contains the right-hand animation controller, which integrates with player movement and abilities.

The code references systems outside this selection, including player movement, camera look, status, abilities, highlighting, left-hand animation, UI, input management, and pause management. Input uses the Unity Input System and the project's generated `PlayerControls` class. Scenes, prefabs, Animator controllers, animation clips, and other assets are not included.

The shared damage contract and feedback events can be inspected in the existing turret selection:

- [IDamageable](../TurretSystem/Shared/IDamageable.cs)
- [GameEvents](../TurretSystem/Shared/GameEvents.cs)

To try the game, download a [playable build](https://github.com/ValiMarin/Xaryon/releases/tag/v0.1.0).
