# RoboCube

*A solo-developed soulslike action-adventure in Unity 6. Every system and every asset built by one person.*

[![Progress footage](https://img.shields.io/badge/▶_Watch-progress_footage-FF2E63)](https://youtu.be/V6h2Ct0Dcvs)
[![Case study](https://img.shields.io/badge/Read-case_study-2D50C8)](https://rohan-samuel.github.io/projects/robocube.html)
![Unity](https://img.shields.io/badge/Unity-6000.5.9f1-black)
![Language](https://img.shields.io/badge/C%23-100%25-239120)

![Turret tracking and the overheat bar filling, then decaying as the player backs off](./media/gameplay.gif)

*Six seconds of the core loop: the turret tracks while the heat bar (red, top-left) fills, then
decays once fire stops. [Full-quality clip](./media/gameplay.mp4).*

You wake as a lone machine in a decaying network built by other machines, piecing together what
happened to the world from the data fragments left behind. There's no stamina bar. Staying aggressive
builds heat, and heat is what forces you to back off.

The project is in pre-alpha and under active development. The playable core works: movement, camera,
the stat and overheat systems, a turret weapon with aim tracking and projectiles, and multi-slot
save/load. Enemy AI and damage resolution are what I'm building now.

## Play it

Grab the latest `build.rar` from [Releases](../../releases), extract, and run the `.exe`.

To open the project instead, you need Unity 6000.5.9f1. Clone it, open the folder through Unity Hub,
and load `Assets/Scenes`. The `Library/` folder isn't committed; Unity rebuilds it on first open,
which takes a few minutes.

## Architecture

A character is a thin state object surrounded by single-responsibility managers. The player is a
subclass that overrides only what differs.

```
CharacterManager              ← flags, stats, resources, death handling
├── CharacterLocomotionManager
├── CharacterCombatManager
├── CharacterAnimatorManager
├── CharacterEffectsManager
├── CharacterStatsManager
├── CharacterInventoryManager
└── CharacterEquipmentManager
        ▲
        │ inherits + overrides
        │
PlayerManager                 ← adds input, camera, UI, save serialisation
├── PlayerLocomotionManager
├── PlayerCombatManager
├── PlayerInputManager
├── PlayerCamera
├── PlayerUIManager
└── ...
```

`CharacterManager` owns the flags every character needs (`isPerformingAction`, `canRotate`,
`isGrounded`, `isLockedOn`) and the resources they all track. Subsystems read those flags instead of
reading each other, so behaviours compose without managers holding references to one another. An
attack sets `isPerformingAction`, and locomotion, stat regeneration and the animator each respond on
their own.

Above the characters sit four world singletons: `WorldSaveGameManager`, `WorldItemDatabase`,
`WorldSoundFXManager` and `WorldCharacterEffectsManager`.

## Engineering notes

### The overheat system

Most soulslikes drain a pool you spend. RoboCube inverts it: overheat accumulates from action and
decays once you stop. `CharacterStatsManager.RegenerateOverheating()` returns early while
`isSprinting` or `isPerformingAction` is true, then after a 2-second delay bleeds heat off on a 0.1s
tick. The punishment for over-committing lands once you stop, which makes backing off something you
have to time.

Stats feed resources through a single conversion point: `durability` sets max health and `coolant`
sets max heat. Rebalancing means changing one formula.

### Weapons as data

`WeaponItem` is a `ScriptableObject` carrying a model, level requirement, damage and poise values,
and an action reference. The action is also a ScriptableObject (`WeaponItemAction`, subclassed by
`LightAttackWeaponItemAction`), so adding a weapon means authoring an asset in the editor and
assigning its behaviour, without touching the combat code. The turret is the first weapon built on it.

### Projectile collision

A fast projectile moved with `transform.Translate` will tunnel straight through thin colliders on a
low frame. `RaycastProjectile` raycasts from last frame's position along exactly this frame's
movement distance before it moves, so a hit registers even when the travel distance is greater than
the target's thickness. Each bullet also destroys itself after its lifetime, so missed shots can't
leak.

### Aim and animation

The robot's body aim uses Unity's Animation Rigging `MultiAimConstraint`, which blends aiming over
whatever locomotion animation is playing. `SyncAimTarget` then copies the body's current aim target
onto the turret's own constraint each frame, so the turret and the body track the same point. It
early-outs when the target hasn't changed.

### Saving

`CharacterSaveData` is a serialisable POCO holding name, seconds played, world position, stats and
current resources. `SaveFileDataWriter` writes it through `JsonUtility` to a per-slot file on disk
and handles slot-occupancy checks and deletion. Human-readable saves are easy to inspect and edit
while the game's data model is still changing.

## Built with

| Tool | Used for |
|------|----------|
| Unity 6 (6000.5.9f1) | Engine, gameplay scripting, level assembly |
| C# | All gameplay code |
| Unity Input System | Rebindable input via generated `PlayerControls` |
| Unity Animation Rigging | Aim constraints and turret tracking |
| Maya | Modelling, rigging, animation |
| Substance Painter | Texturing |
| Photoshop | UI and concept work |
| FL Studio | Audio |

## Roadmap

| Area | State |
|------|-------|
| Third-person movement & camera | Working |
| Stat system (durability / coolant) | Working |
| Overheat accumulation & decay | Working |
| Weapon & item framework | Working |
| Turret aiming + projectiles | Working |
| Multi-slot save / load | Working |
| Title screen & save-slot UI | Working |
| Enemy AI | In progress |
| Lock-on targeting | Input wired, behaviour in progress |
| Damage resolution | In progress |
| Environment layout | In progress |
| Narrative & data fragments | Planned |

## Credits

Everything here is mine: all gameplay code, the player character's model, rig, textures and
animations, and the project's design. It's a portfolio piece built to be full-stack on purpose, so I
own every layer of it.

Built by Rohan Samuel. [Portfolio](https://rohan-samuel.github.io) ·
[LinkedIn](https://www.linkedin.com/in/r-samuel/) · rohan2309@gmail.com
