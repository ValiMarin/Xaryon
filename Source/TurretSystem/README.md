# Turret System

Selected source code from Xaryon, a sci-fi first-person shooter developed in Unity.

The system combines shared turret behaviour with specialized enemy variants. Detection, shooting, animation, audio, visual feedback, and health are handled by separate components.

## Start Here

- [TurretController](Turrets/TurretController.cs) — coordinates detection events, state changes, shooting, and death.
- [TurretDetection](Turrets/TurretDetection.cs) — handles line-of-sight checks, detection delay, and proximity-based standby behaviour.
- [TurretShooting](Turrets/TurretShooting.cs) — provides the base projectile-firing behaviour.
- [TurretAnimationHandler](Turrets/TurretAnimationHandler.cs) — connects animation events to gameplay actions.

## Behaviour Flow

1. Detection checks whether the player is visible and within range.
2. Detection events notify the controller.
3. The controller selects Idle, Shooting, or Standby.
4. The animation handler updates the Animator.
5. A shooting animation event notifies the controller.
6. The controller invokes the shooting component and plays audio and visual feedback.

This keeps projectile creation synchronized with the firing animation.

## Design

### Encapsulation

The controller keeps its state and component references internal. Other components communicate through initialization methods, focused public methods, and events.

### Inheritance and Polymorphism

The shared components provide extension points for specialized behaviour:

- [SniperTurretShooting](Turrets/Sniper/SniperTurretShooting.cs) overrides `Shoot()` to use raycast-based damage instead of a physical projectile.
- [BomberController](Turrets/Bomber/BomberController.cs) extends initialization and death behaviour while retaining the base controller logic.
- [BomberProjectile](Turrets/Bomber/BomberProjectile.cs) and [PenetratorProjectile](Turrets/Penetrator/PenetratorProjectile.cs) specialize projectile behaviour.

### Interfaces

[EnemyHealth](Shared/EnemyHealth.cs) implements [IDamageable](Shared/IDamageable.cs), allowing damage sources to interact with a common contract.

[IHighlightable](Shared/IHighlightable.cs) provides a separate contract for highlighting.

### Composition and Events

Specialized behaviour can also be added through components.

[PenetratorBehaviour](Turrets/Penetrator/PenetratorBehaviour.cs) subscribes to detection and health events to control its spike mechanism without extending the turret controller.

### ScriptableObjects

Turret settings, projectile settings, and animation speeds are stored separately from runtime behaviour:

- [TurretSO](Turrets/TurretSO.cs)
- [TurretProjectileSO](Turrets/TurretProjectileSO.cs)
- [TurretAnimationProfileSO](Turrets/TurretAnimationProfileSO.cs)

## Scope and Dependencies

This folder is a source-code showcase extracted from the game, not a standalone Unity project.

Some scripts reference systems from the full project, including player status, loot, damage popups, and damage indicators. Prefabs, scenes, animations, and other assets are not included.

To try the game, download a [playable build](https://github.com/ValiMarin/Xaryon/releases/tag/v0.1.0).
