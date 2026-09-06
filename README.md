# "In The Forest" – Unity / C# Game Project

Started as a solo **Global Game Jam 2026** project, this repository showcases my transition from a Senior Art/Production background into **Gameplay and Client Programming** in Unity/C#. 

The goal of this project was to implement clean, scalable code, focus on architecture, and build reusable gameplay systems from scratch.

---

## 🛠️ Key Programming Features Implemented

*   **Procedural Maze Generation:** Implemented a grid-based maze generation system using a randomized backtracking algorithm.
*   **Event-Driven Systems:** Built an architecture utilizing C# events/actions to handle decoupled communication between NPCs, world interactions, and game state changes.
*   **Object Pooling:** Created an efficient Object Pool for managing collectible pickups to avoid garbage collection spikes and runtime instantiation overhead.
*   **Dynamic Visual Shifting (Mask System):** Coded a system where equipping a mask item alters camera culling masks and triggers post-processing changes dynamically, revealing hidden scene layers and ghost characters.
*   **Breadcrumb Mechanic:** Implemented logic allowing players to throw, track, and re-collect items, providing a dynamic navigation aid through the matrix-based labyrinth.

---

## 🧠 Technical Highlights & Code Patterns

*   **Decoupling:** Strict separation of data (ScriptableObjects), logic (C# Managers), and presentation (MonoBehaviours).
*   **Input System:** Built using Unity's New Input System for clean, action-based player controls.
*   **State Management:** Basic state transitions for player behaviors and gate states.

---

## 🕹️ Play the Game
The latest build is compiled and playable on **[Itch.io](https://babybottlestudio.itch.io/in-the-forest)**.

---

## 💻 Tech Stack
*   **Engine:** Unity 6
*   **Language:** C# (OOP, Events, Data Structures)
*   **Version Control:** Git

  
## 📄 License & Copyright

Copyright (c) 2026 Miljan Novčić. All rights reserved.

This repository and its contents are strictly for portfolio demonstration and code review purposes. No part of this project (including code, architecture, or assets) may be downloaded, copied, modified, or distributed for personal, educational, or commercial use without explicit permission.
