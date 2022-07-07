# Groundbreak

Groundbreak is a 2D turn-based procedurally generated roguelike in which the player can pick up and throw the tiles that make up a room to fight the enemies. Based on the thrown tile and target tile, various elemental effects are created that can damage and impair the enemies. The player proceeds through the floor, fighting enemies, gathering items, and eventually reaches a unique boss that they must defeat to progress to the next floor. After arriving at the second floor, the player fights new enemies, finds new items, and must beat another unique boss to win the game. There are a total of 8 regular enemies to fight, as well as the two unique bosses, both with unique mechanics. The level one boss can block thrown tiles and reflect them back to you, while the level two boss's attacks push the player around the room.

The final built game is in the zipped folder named "Groundbreak Final build," and the raw Unity game binaries are in the folder just titled Groundbreak.

Controls:
W A S D → Move player in free roam, pan camera in combat
Right click → pick up 
Left click → move or throw tile or use active item based on which mode the user is in (3 buttons on bottom of screen)
BackSpace → center camera at player in combat
Space → Elemental Overlay
i → Inventory
Tab —> UI overlay and quick reference 
Esc → Pause menu


Reactions (these are also in the quick reference that you access with tab) : 

Fire + Water = Smoke, lowers the attack range of enemies, light damage
Fire + Earth = Magma, creates hazard that damages whoever walks onto it, high damage
Fire + Air = Fireball, Area of Effect Damage, Moderate Damage
Water + Earth = Mud, Stops enemies and lowers movement, light damage
Water + Air = Storm, Pull enemies close causing additional damage if they collide with another, light to moderate damage
Earth + Air = Sandstorm, Push enemies away causing additional damage if they collide with another, light to moderate damage.
Same Element + Same Element: Spread the Element to all surrounding tiles, but consume the middle tile
