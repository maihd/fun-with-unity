# Gameplay simulation patterns and solutions


## Design goals
- Game state is single truth
- Game state is double buffering 
- Easy testing and doing TDD
- Easy to make a replay systems
- Can use custom languages, any languages even in procedural languages
- Must be doing in procedural programming. No allocation when game simulation.
- Event are handle with event queue and event loop. Avoid callbacks.
- Coroutine works, but as a stateless version, or manually state machine.
- Seperate gameplay logic with rendering, audio, UI, ...
- Game engine works as platform layers


## Architecture
```mermaid
Game simulation -> Systems (Unity, or any game engines) -> OS
User interactive -> Game engine UI framework -> Game simulation
Game simulation update -> Game engine data sync & handling events through event queue -> Game engine rendering 
```