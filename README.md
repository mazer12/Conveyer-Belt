# Conveyor Puzzle Prototype

## Overview

Prototype assessment. The player must guide the item from the start point to the correct end hole by using interactable mechanics.
The game contains three levels with increasing difficulty.


##To play

The game starts at Level1, press SPACE to start the level (item will start moving in belt), press the button to turn (remember to press the button exactly when the item stops for less than a second, otherwise you will fail). 

## Architecture

The project uses a simple one-scene level structure. Three levels managed by LevelManager


Main systems:

* `GridManager` registers conveyors, holes, and teleporters based on world X/Z grid positions.
* `ItemMover` moves the item deterministically from one grid cell to the next.
* `ConveyorTile` defines the movement direction for normal conveyor tiles.
* `TurnTile` extends conveyor behavior and allows direction changes during a timed input window.
* `TurnButton` sends input to one or more turn tiles.
* `BlackHole` handles win/loss based on whether the item reaches the correct hole.
* `Teleporter` moves the item from one entry point to another exit point.
* `LevelManager` handles level progression, reset, and loading the next level.

## Tradeoffs

The prototype prioritizes simple, reliable gameplay logic over visual polish. The grid is logical rather than made from individual floor tiles, which keeps setup fast and easy to debug. Movement is not physics-based; the item moves directly between grid centers for predictable puzzle behavior.

Levels are handled inside one scene instead of using multiple scene loads. This keeps the assessment project small and easy to inspect, but it is probably less scalable for a larger game.

## Improvements With More Time

With more time, I would improve the project by adding clear level transition screens, visual effects for teleporters and holes, and stronger more fun puzzles. I would also add a proper level data system, more puzzle mechanics, sound effects, and cleaner win/loss presentation. Right now the each level has only one box/item/luggage but I will add a continuous stream of items in a more complete game.
