# Shift-It
A 2d Platformer game with color-swtiching

## Collectables

Two basic colored circles are placed beside the player in `MainLevel`:

- Green: touch to collect, then press **K** to freeze platform break countdowns for **5 seconds**. Player movement continues normally.
- Orange-red: touch to collect, then press **L** to reveal hidden color blocks for **5 seconds**. Revealed blocks are translucent hints; their collision rules do not change.

Collected items disappear from the level and appear in the top-left inventory. Each activation consumes one item. The HUD shows the remaining effect time, and used icons disappear when the effect ends. Restarting the level resets pickups and inventory.

The durations can be adjusted on the player's `CollectableInventory` component. The two pickup prefabs are in `Assets/Prefabs`.
