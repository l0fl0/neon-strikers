# NeonStrickers
# Neon Strikers

## Project Overview

Neon Strikers is a 2D arcade-style multiplayer game built in Unity. Two players (Blue and Red) compete in neon-themed arenas using physics-based movement and interactions to score goals.

---

## How to Open the Project

1. Install **Unity Hub**
2. Install **Unity 6 (or compatible version used in this project)**
3. Open Unity Hub
4. Click **Open Project**
5. Select the project folder (`neon-kickers`)
6. Open the scene:

   ```
   Assets → Scenes → Main Menu
   ```
7. Press **Play** to start the game

---

## How to Build the Game

1. Open the project in Unity

2. Go to **File → Build Settings**

3. Add the following scenes **IN THIS ORDER**:

   * Main Menu
   * Arena_01
   * Arena_02
   * Arena_03
   * Results

4. Select your platform (Windows or Mac)

5. Click **Build**

6. Choose a folder to export the game

---

## Project Structure

### Scenes

Located in: `Assets/Scenes/`

* `Main Menu` → Main menu UI and navigation
* `Arena_01`, `Arena_02`, `Arena_03` → Playable arenas
* `Results` → Displays winner and final score

---

### Scripts

Located in: `Assets/Scripts/`

#### Gameplay

* `GameManager` → Controls match flow and scoring
* `GoalZone` → Detects when a goal is scored
* `MatchData` → Stores score and winner between scenes
* `ResultsManager` → Handles Results screen (background swap + ghost animation)
* `SceneLoader` → Handles scene transitions

#### Player

* `PlayerMovement` → Player movement and controls
* `PlayerKick` → Ball interaction (kicking)
* `PlayerLoseEffects` → Visual effects when losing
* `PlayerLoseLaunch` → Launch/knockback effect on loss
* `PulseEffect` → Visual pulse interaction effect

#### Ball

* `BallController` → Handles ball physics and behavior

#### Camera

* `CameraShake` → Camera effects

#### UI

* `LogoPulse` → Logo animation in Main Menu
* `UIButtonHover` → Button hover effects

#### Utilities

* `GoalSlowMotion` → Slow-motion effect on goal
* `LocalSlowZone` → Area-based slow zones
* `PortalCooldown` → Portal cooldown logic
* `PortalTeleport` → Teleportation system
* `ZoneSlowable` → Controls slowdown zones

---

## Gameplay Features

* 2-player competitive gameplay
* Physics-based movement and ball interaction
* Multiple arenas
* Neon visual style
* Portal system and slow-motion zones
* Goal-triggered effects
* Results screen with dynamic background:

  * Blue wins → Blue victory background
  * Red wins → Red victory background
* Ghost animation appears above the losing player

---

## Controls

### Player 1 (Blue)

* Move: **W A S D**
* Action: **Space**

### Player 2 (Red)

* Move: **Arrow Keys**
* Action: **Enter**

---

## Game Flow

1. Main Menu → Start Game
2. Arena (01 / 02 / 03) → Players compete
3. Goals are detected using `GoalZone`
4. Match ends → `MatchData` stores results
5. Results Scene:

   * Background changes based on winner
   * Final score is displayed
   * Ghost animation plays on losing side
6. Player chooses:

   * Play Again
   * Return to Main Menu

---

## Notes

* Make sure scenes are added to Build Settings **in the correct order**
* Results scene must be last
* Background image is used instead of winner text
* Ghost animation is handled separately from background
* Time scale is reset in the Results scene

---

