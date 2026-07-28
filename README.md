# BombGame 💣

A grid-based action game inspired by the NES Bomberman mechanics, developed to practice 2D game architecture and state management in Unity 6.5. A hybrid system used in this game where player and enemy collisions depend on engine physics, and other mechanics depend on the grid data.

### Video Demo: 

https://github.com/user-attachments/assets/aa8fe3ee-56fe-4ca9-ab50-2f500d9db001

## 🎮 Features
* Collision detections with Unity Physics Raycast.
* Dynamic UI to demonstrate game information.
* Random map creation.
* Random enemy and item spawner.
* Game over and winning states.
* Object pool to increase game performance.
* 2D grid data to manage grid.
* Bomb explosions that break brick, kills entities and explode other bombs.

## 🛠️ Architecture & Project Post-Mortem
This project was developed as an early prototype and a learning exercise. It is considered complete and will remain as-is without future architectural updates.

While developing this game, I identified several architectural bottlenecks. Rather than spending time refactoring this specific codebase, I am applying the following lessons to my future projects:

* Coupled Architecture: Current systems have tight dependencies.
* SOLID Principles: Certain classes currently handle multiple responsibilities, making the code harder to scale.

