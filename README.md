# MiniTowerDefense

Project with the following premise: *mini tower defense where the player must defend a core structure from 5 waves of enemies, 
by constructing course obstacles and offensive tower to destroy them*.

This project is made in **Unity 2019.1.14f1** and scripted in **C#**.

**Initially the project should attend the following criteria**:
1. Have a construction system (to build barricades and towers) that allows players to:
   1. visualize the future construction position, 
   2. confirm/cancel the construction of a new structure, and 
   3. confirm/cancel the destruction of a existent structure.

2. Have a enemy AI system that:
   1. direct the enemy from a specific spawn point to the core structure (their final objective), 
   2. controls its health info and shows it to the player in a form of health bar, and
   3. do not attack constructed towers nor defend itself from them.

3. Have a spawn enemy system that allows to:
   1. spawn enemies from different spawn points in each wave,
   2. spawn enemies in different formations (at least one from each: individual and group).

**The game itself have the following requirements**:
1. It is supposed to be in 3D,
2. The demo game should have a completed game loop (beggining to end),
3. The demo game consist of 5 enemy waves with time triggers.

**This project had the following liberties**:
1. The arena could be in any format,
2. Visuals and UI were not important - *so were not focused by me*,
3. It was possible to use external assets - *but I opt to program all by myself and only use Unity capabilities*.

My take on this endeavor:
=========================
Inspired by old tower defense flash games like *[Desktop Tower Defense](https://armorgames.com/play/1128/desktop-tower-defense-15)* and motivated by the 3D nature of the project, I decided to do a circular laneless arena with free structure positioning. The enemies spawn at random in 8 possible points and the player must defend himself from a 360° angle. The enemies hurt the core structure with the remaining life they have left.

To play this little demo:
=========================
The [GameDemo.zip](https://github.com/epinheiro/MiniTowerDefense/blob/master/GameDemo.zip) is the zipped game build and can be promptly played executing the ***MiniTowerDefense.exe*** after unzip it.

Technical project highlights:
==============================

**A [GameSetup scriptable object file](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Resources/Data/GameSetup.asset)  ([GameSetup.cs](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Data/GameSetup.cs)) is used to describe some game variables as**:

+ *Pool sizes from*: tower, wall, enemy, and projectile;
+ *Core attributes*: total life, and period to check for nearby enemies in seconds;
+ *Tower behaviour*: total of enemies that they can simultaneously attack, and period to check nearby enemies in seconds;
+ *Projectile attributes*: movement velocity.

**All construction (tower and wall), enemies and projectiles use a auxiliary [PoolingSystem](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Auxiliary/PoolingSystem.cs) and manage it accordingly with their needs:**

+ [ConstructionSystem](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Systems/ConstructionSystem.cs) use their pool size to control the maximum of each structure that can be created, demanding the UI controller to hide the buttons whenever the pool is completely used for each of them;

+ [EnemySystem](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Systems/EnemySystem.cs) use their pool size but instantiate the necessary enemies if the pool is full and the [SpawnSystem](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Systems/SpawnSystem.cs) wants to insert more enemies on screen - this behaviour was added because the program is programmatically set up with config data files, so it is important to the game adapt accordingly;

+ [ProjectileSystem](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Systems/ProjectileSystem.cs) use their pool size but increase the pool one bullet at time, because the necessities are less aggressive than the enemy spawn system's needs.

**The spawn behaviour obeys a main scriptable object named [Game](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Resources/Data/Waves/Game.asset) ([GameWaveDefinition.cs](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Data/Waves/GameWaveDefinition.cs)) that aggregates other scriptable objects**:
1. Each [GameWaveDefinition.cs](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Data/Waves/GameWaveDefinition.cs) has a list of [WaveData.cs](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Data/Waves/WaveData.cs), that describes the *macro waves* expected in the game;
2. Each [WaveData.cs](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Data/Waves/WaveData.cs) has a list of [EnemyWave](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Data/Waves/EnemyWave.cs), that describe the *sub waves* of enemies expected in that *macro wave* - the total time is the sum of the *sub waves* individual times;
3. Each [EnemyWave](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Data/Waves/EnemyWave.cs) describes the expected *sub waves* attributes:
   1. *timeUntilNextSubWave*: time in seconds to [SpawnSystem](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Systems/SpawnSystem.cs) spawns more enemies;
   2. *enemyType*: which of the [EnemiesAttribues](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Data/Units/EnemyAttributes.cs) ([Fast](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Resources/Data/Units/Enemy1.asset) or [Strong](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Resources/Data/Units/Enemy2.asset)) that will be spawned in this *sub wave*;
   3. *quantity*: number os enemies spawned;
   4. *formation*: how the enemies will appear for the players (as described in the [SpawnSystem](https://github.com/epinheiro/MiniTowerDefense/blob/master/Assets/Scripts/Systems/SpawnSystem.cs) as *DelayedIndianLine* and *LineGroup* - respectively instantiated in methods *SpawnDelayedSingleEnemy* and *SpawnEnemyLineGroup*).
