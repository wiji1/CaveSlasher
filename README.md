# Cave Slasher
Cave Slasher is a 2D dungeon crawler game developed in Unity. The game serves as a demo project for helping me learn Unity game development

## Game Overview
In Cave Slasher, players navigate through carefuly designed levels with the goal of reaching the exit while avoiding traps and enemies. The game features a simple control scheme, allowing players to move their character left and right using the ```A``` and ```D``` keys respectively. The player can jump with ```SPACE``` and attack with ```X```. Each level has an exit door that is only unlocked once the player has obtained every coin in the level. Enemies will block the path of these coins, facing the player with a combat challenge. 
<br>
<br>
Cave Slasher's gameplay is designed to be straightforward and engaging, with a focus on exploration and combat. While only a couple of levels have been implemented so far, the game is designed to increase in difficulty as the levels progress, eventually introducing more complex enemy types and environmental hazards.

## Technical Implementation
Cave Slasher is built using Unity, leveraging its powerful 2D game development tools. The game utilizes Unity's physics engine for realistic movement and collision detection. The player character and enemies are controlled through scripts written in ```C#```, which handle movement, combat, and interactions with the environment. The game's scripts are designed in a way to make development extremely easy, with interfaces existing for most game objects to allow for easy modification and extension. A large portion of the game's combat system uses advanced ```OOP``` to allow for easy addition of new enemy types, attack patterns, animations, obstacles, and more.
<br>
<br>
The game incorperates a proper Level Management system for the loading of different scenes, complete with a Title Screen and Level Selection Screen. The level management is performed using a ```GameObject``` that persists across scenes with an attached script that handles scene loading and transitions.
<br>
<br>
All sprites in the game are free assets obtained from [itch.io](https://itch.io/game-assets) and and the [Unity Asset Store](https://assetstore.unity.com/). Most sprites include animations implemented using a combination Unity's Animator system and a custom-implemented ```AnimationController``` script that allows for our game objects to easily switch between different animations based on their current state.
<br>
<br>
While a large portion of the game's development efficiency infrastructure is implemented through the ```C#``` scripts, the game also makes use of Unity's Prefab system to allow for easy duplication and modification of game objects. Most game objects in Cave Slasher, such as the Player, enemies, coins, and exit gates are implemented as prefabs, allowing for the fast creation of new levels.
<br>
<br>
The game also makes use of ```Colliders``` to handle collisions between the player, enemies, and environment. Different types of colliders are used for different game objects, such as ```BoxColliders``` for the player and enemies, and ```PolygonColliders``` for more complex shapes like the coins. The colliders are configured to ensure accurate collision detection while maintaining optimal performance. Our scripts interact with ```Trigger``` colliders to handle events such as taking damage, collecting coins, and reaching the exit.
<br>
<br>
Cave Slasher also incorporates a simple UI system to display important information to the player, such as the number of coins collected and the player's health. The UI is implemented using Unity's built-in UI tools, which allows for easy customization and scaling across different screen sizes. The game also incorperates more complex UI elements, such as the spinning coin icon that displays the number of coins collected, which is implemented using a combination of ```Canvas``` and ```RenderTexture``` components along with custom scripts to handle the spinning animation and updating the coin count.
<br>
<br>
Sprite movement is present through both the player and enemies. The player movement is handled through a combination of physics-based movement and direct manipulation of the player's ```Transform``` component. Enemy movement is handled using a simple AI pathfinding system that uses its proximity from the player to decide when to move towards the player, stop, and attack.
<br>
<br>
GameObjects are dynamically spawned and destroyed throughout the game. Enemies are spawned at the start of each level based on their placement in the scene, while coins are spawned through a combination of pre-placed coins in the scene and coins that are spawned when an enemy is defeated. GameObjects are destroyed when they are no longer needed, such as when an enemy is defeated or when the player collects a coin.
<br>
<br>
The game has been built for desktop platforms, with executables available for both Windows and MacOS on the [Releases Page](https://github.com/wiji1/CaveSlasher/releases). There is also a version hosted on [Unity Play](https://play.unity.com/en/games/032d5499-0ef0-4ba2-bb32-1b0bb16b7a93/sprite-flight).

## Future Development
Cave Slasher is still in the early stages of development, with many features and levels yet to be implemented. Future development plans include:
- Adding more levels with increasing difficulty and complexity.
- Introducing new enemy types with unique behaviors and attack patterns.
- Implementing power-ups and special abilities for the player.
- Enhancing the game's graphics and animations for a more polished look.
- Adding new combat and movement mechanics to increase gameplay variety.

Additionally, some sort of overarching story or theme could be added to give the game more context and immersion.

## Development Reflection
Developing Cave Slasher has been a valuable learning experience in Unity game development. The project has allowed me to explore various aspects of game design, including level design, character control, enemy AI, and UI implementation. Through this project, I have gained a deeper understanding of Unity's capabilities and best practices for game development. In addition, it has also helped me learn the syntax of ```C#``` and how to effectively use it within the Unity environment.
<br>
<br>
The most challenging aspect of the development process has been balancing the game's difficulty and ensuring that the gameplay remains engaging and fun. While I generally find it quite easy to implement complex systems and mechanics, designing levels that are both challenging and engaging is something I still need to work on.
<br>
<br>
While only a "Technical Demo", if I were to continue development on Cave Slasher, I would focus more on the gameplay and less on the technical implementation under the hood. However, I do think that because of the amount of time I put into the game's infrastructure, this would be fairly trivial.