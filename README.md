# Bowling Game with Unity

This project is a bowling game built-in Unity, featuring gameplay mechanics for single-player bowling. The game transitions to the next level when the player knocks down all pins, with proper turn management and logic handling for both first and second turns.

- **Itch.io Page**: [Strike Bowling on Itch.io](https://tomgoz.itch.io/strikebowling)

---

## Features

- **Turn-Based Mechanics**: The player has up to two turns per level to knock down all pins.
- **Level Progression**: Automatically advances to the next level upon clearing all pins just in the first turn the make the game more difficult.
- **Reset Mechanism**: Restarts the current level if the player fails to knock down all pins after two turns.
- **Realistic Bowling Mechanics**: Includes side-to-side movement of the ball before the throw and realistic knockdown physics.
- **Multi-Level Gameplay**: Handles progression across multiple levels with varying difficulties.
- **Camera Vision**: Main camera followed the ball through his path.

---

## Usage

1. **Start the Game**:
   - Press `Enter` to throw the ball toward the pins.
   - Before pressing `Enter`, the ball moves side-to-side to aim.

2. **Turn Management**:
   - Players get two turns per level to knock down all pins.
   - If successful within the first turn, the game transitions to the next level.
   - If unsuccessful, the level restarts.

3. **Win Condition**:
   - Knock down all pins within one turns to progress to the next level.

---

## Code Overview

### GameManager.cs

This script manages the core game mechanics, including:
- Turn logic
- Pin state checks
- Ball throwing and resetting
- Level transitions

**Key methods**:
- `CountPinsDown`: Updates the score and marks knocked-down pins as inactive.
- `CheckAllPinsKnockedDown`: Determines if all pins are down to trigger a level transition.
- `HandleTurnResults`: Manages logic for turns and transitions.
- `LoadNextLevel`: Loads the next scene from the build index.

---

## Levels

### Level One
In level one, the pins are positioned closer, providing an easier challenge for beginners.

<img width="956" alt="Level One Screenshot" src="https://github.com/user-attachments/assets/39f2fef9-bf97-4961-93cd-9fcf93de89af" />

---

### Level Two
In level two, the pins are placed farther apart, increasing the difficulty for more advanced players.

<img width="956" alt="Level Two Screenshot" src="https://github.com/user-attachments/assets/0ba1aa10-d635-4164-9c36-dd35c8073634" />
