# CS425_Final-Project

The player is trapped in a maze-like facility and must blindly find the exit while avoiding numerous traps that will kill them if activated. A burn mark of the explosion that killed the player is left behind, and they must utilized to safely navigate the maze.

An interesting component of this game is that the player is required to die to be able to progress and find the safe path out of the maze. Also, the difficulty and size of the maze increases as levels are completed; the spawn rate of traps is increased after each level, and the size is increased at every third level. The trap spawn rate is also reset every third level to avoid the maze becoming too difficult.

# Technical Component

This game utilizies procedural generation to create the maze and to place the traps within it. The player goal is also randomly placed within the maze with a higher chance of the exit being placed farther away from the player. Breadth-first search is used to ensure a valid path from the start to the goal of the maze.

# Engine

Unity is the game engine being used.

# Goal

Find and get to the green cube marking the exit of the maze.


# How to Play

Use WASD to move and the mouse to control the camera. If the player steps on a trap tile, they will die and respawn at the start. Burn marks from the explosive traps are left behind that the player can use to safely navigate the maze. Continue exploring the maze until a safe path to the exit has been found and the goal is reached. A new maze will be generated with increased There is no penalty for dying.


# Video

Youtube: https://youtu.be/hK4K1_T9kVM
