# Loot and Pickup UI

Selected source code from Xaryon, a first-person shooter developed in Unity.

This selection follows an item from loot generation and collection to an on-screen notification. Repeated pickups of the same type are grouped into reusable UI slots.

## Start Here

- [PickupUIManager](UI/PickupUIManager.cs) — selects notification slots and groups repeated pickups.
- [PickupUISlot](UI/PickupUISlot.cs) — manages a notification's icon, count, and remaining display time.
- [PickupItem](Loot/PickupItem.cs) — handles world movement, collection, rewards, and the UI notification request.
- [LootDropper](Loot/LootDropper.cs) — applies the configured drop chance and selects an item prefab.
- [PickupSO](Loot/PickupSO.cs) — stores the pickup type, reward amount, and additional pickup data.
- [PickupType](Shared/PickupType.cs) — identifies item categories.

## Collection Flow

1. A gameplay system calls `LootDropper.TryDrop()`.
2. The dropper checks its drop chance and randomly selects one of its configured prefabs.
3. The spawned pickup receives an upward impulse, rotates, and checks for the ground.
4. After landing, it can start moving toward the player's pickup target when the player enters its trigger and the visibility check permits collection.
5. The pickup accelerates toward the target. At close range it applies its reward and calls `PickupUIManager.ShowPickup(type)`.
6. Collection audio is detached so it can finish playing after the pickup is destroyed.

Reward handling in the current selection covers ammunition and battery healing. The coin enum entries do not currently grant a reward.

## Notification Flow

For each pickup notification, the manager:

1. Looks up the icon associated with the pickup type.
2. Searches for an active slot already displaying that type. If found, it increments the count and resets the display timer.
3. Otherwise, uses the first empty slot.
4. If every slot is occupied, replaces the first slot in the configured array.

The fallback replaces array index zero; it does not search for the oldest notification.

Each frame, the manager advances the slots' timers using unscaled delta time. When a timer expires, the slot clears its text, hides its icon, and becomes available again. Notification timers therefore continue while the game's time scale is zero.

## What the Counter Represents

`x3` means three collected pickup objects of the same type, not three units of the reward.

The reward amount comes from `PickupSO.amount` and is applied separately. For example, if each ammunition pickup grants 10 rounds, collecting three grants 30 rounds while the notification displays `x3`.

## Design

- Collection and rewards are handled by the world pickup; notification allocation and display state are handled by UI components.
- The UI manager exposes a small `ShowPickup(PickupType)` entry point.
- Each slot encapsulates its active state, count, and timer.
- Pickup configuration uses ScriptableObjects; icon mappings are configured on the manager and cached in a dictionary.
- The manager operates on a fixed array of existing slots. Showing a notification reuses those objects instead of instantiating new UI objects.

The world loot objects still use `Instantiate` and `Destroy`; slot reuse describes the notification UI.

## Scope and Dependencies

This is a source-code showcase extracted from the game, not a standalone Unity project.

The UI uses Unity UI (`Image`) and TextMeshPro (`TMP_Text`). The world pickup expects a Rigidbody, colliders, the project's tags and physics layers, and a target tagged `PickupTarget`.

References to player health and pause management (`PlayerStatus` and `GameStateManager`) belong to the full project and are not included here. Related ammunition code is available in the player combat selection:

- [PlayerAmmo](../PlayerCombat/Shared/PlayerAmmo.cs)
- [ShootingMode](../PlayerCombat/Shared/ShootingMode.cs)

Scenes, prefabs, ScriptableObject instances, sprites, audio, and Canvas configuration are not included.

To try the game, download a [playable build](https://github.com/ValiMarin/Xaryon/releases/tag/v0.1.0).
