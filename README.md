# Essedge
(This is an academic exersize.)

A 2-player cooperative top-down medieval fantasy survival game built in Unity 6. Fight through waves of enemies, survive 3 rounds, and see who contributed most on the results screen.

---

## Setup Instructions

### Requirements
- Unity 6 (Universal 2D Render Pipeline)
- Unity Netcode for GameObjects
- ParrelSync (for local multiplayer testing)

### How to Clone and Open
```bash
git clone https://github.com/Nate-Cold/GDEssedge.git
```
1. Open Unity Hub
2. Click **Add project from disk**
3. Navigate to the cloned `GDEssedge` folder
4. Open the project in Unity 6

---

## How to Test Multiplayer

### Using ParrelSync (Recommended)
1. Open **ParrelSync → Clones Manager** from the Unity toolbar
2. Create a clone of the project
3. Open the clone window
4. Hit **Play** in both editors
5. Click **Start Game** in one editor (Host)
6. Click **Join Game** in the other (Client)

### Controls
| Player | Move | Attack |
|--------|------|--------|
| Host | Arrow Keys | Left Click |
| Client | WASD | Left Click |

---

## Scene Order (Build Profiles)
| Index | Scene |
|-------|-------|
| 0 | MainMenu |
| 1 | LobbyScene |
| 2 | HuntScene |
| 3 | ResultsScene |

---

## Technical Requirements Implemented

### 1. Singleton Pattern
**GameManager.cs** — Persists across all scenes via `DontDestroyOnLoad`. Tracks `CurrentRound`, `CurrentWave`, `PlayersAlive`, and end-of-game stats via NetworkVariables.

**AudioManager.cs** — Persists across all scenes, manages background music and SFX with separate AudioSources.

### 2. Delegate Usage
**GameEvents.cs** — Central event hub using delegates. Defines and fires `OnWaveCompleted`, `OnPlayerDamaged`, and `OnPlayerDied` events. Any system can subscribe without direct coupling.

### 3. Additional Design Pattern — Object Pool
**EnemyPool.cs** — Pre-instantiates a pool of enemy GameObjects at game start. Enemies are activated and deactivated between waves rather than instantiated and destroyed, keeping performance stable as wave counts increase.

---

## Key Scripts

| Script | Location | Purpose |
|--------|----------|---------|
| GameManager.cs | Assets/Scripts | Singleton, round/wave logic, game over |
| GameEvents.cs | Assets/Scripts | Delegate event system |
| EnemyPool.cs | Assets/Scripts | Object pool pattern |
| PlayerMovement.cs | Assets/Scripts | Networked player movement via Input System |
| PlayerHealth.cs | Assets/Scripts | Health, damage, kill tracking |
| PlayerCombat.cs | Assets/Scripts | Melee attack via ServerRpc |
| EnemyAI.cs | Assets/Scripts | Enemy movement, targeting, attack |
| WaveSpawner.cs | Assets/Scripts | Wave logic, round progression |
| SpawnManager.cs | Assets/Scripts | Player spawning and positioning |
| AudioManager.cs | Assets/Scripts | Music and SFX management |
| LobbyManager.cs | Assets/Scripts | Ready-up system, scene transition |
| ResultsManager.cs | Assets/Scripts | Results screen, stat display |
| NetworkManagerUI.cs | Assets/Scripts | Host/Client connection buttons |

---

## Known Bugs / Incomplete Features
- Player death does not currently disable player movement or visually indicate the downed state
- Results screen defaults to Victory if only one player dies rather than requiring full party wipe to trigger game over in some edge cases
- Essence currency system referenced in original GDD was cut from scope — Raw, Residual, and Superior Essence are not implemented
- Shop and Arcane Table gambling system cut from scope
- Enemy spawn points are fixed, not procedural

---

## What Remains for Future Development
- SQLite database integration for persistent cross-session stats
- Full essence economy system
- Shop and upgrade system
- Additional enemy types and boss encounters
- 3rd and 4th player support
- Client-side prediction for smoother movement
- Polished art and animations

