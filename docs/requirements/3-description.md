# 3. Description
Overlord-Supreme checkers is a recreation of the classic board game checkers that is intended to be played with 2 players per session. When each player launches the game, they will start in the main menu. From the main menu, they can either open a lobby or join an existing one. Once in the lobby, the player will load into the game scene where there will be a checkers board and will be presented with turn order and other information through the ui.

<div id = figure1 style="text-align:center">

<!-- ![placeholdergamescreen](placeholdergamescreen.png) -->

<!-- **Figure 1:** Player's View of the main menu screen (mockup) -->
</div>

The game itself will be 2-D, and the camera will show a top-down view of the board. The board will consist of the standard 8x8 square grid with offset colored tiles. The theme will be futuristic with hologram looking assets (similar to Tron).

<div id = figure2 style="text-align:center">

<!-- ![placeholderstyleguide](placeholderstyleguide.png) -->

<!-- **Figure 2:**  style mockup -->
</div>

The game will play the same as classic chess where each player will have the goal of eliminating the other's pieces in order to win.

## 3.1 Interface
The interface while in the menu will contain buttons for creating/joining lobbies and exiting the game.<br />
The lobby interface will have a textbox for changing the Player's name, a button for creating a new lobby, and a list of currently open lobbies.
<br />
<br />
<br />
<br />

## 3.2 Functionality
The game will game the following functionality:
- Networked multiplayer with the ability to either host or join a game
- Ability to run the game and restart when over
- Interactable game synced across players

<br />
<br />
<br />
<br />

## 3.3 User Description
The necessary number of players for Overlord-Supreme Checkers is 2 players. 
<br />
<br />
<br />
<br />

## 3.4 Assumptions and Dependencies
### 3.4.1 Unity
Unity is a game development engine that allows users to create video games that can be exported across multiple platforms with relative ease. This project is extremely dependent on Unity functioning and working properly. If Unity ceases support or breaks then the team would be unable to continue. There are other engines that could be used to work on the project but would require a complete rework of almost every single asset in the game. The team is assuming that Unity is not planning on dropping support in the near or distant future.

### 3.4.2 Photon Networking (PUN)
Networking is required for this project and will be using the Photon Networking asset, specifically the PUN protocols. If this functionality breaks, the project would not be able to be finished in time for the end of class. There are other networking solutions but they would require a large amount of work the the Unity Prefabs and structure in order to get working. We assume that Photon is not planning on dropping suppport in the near or distant future.

### 3.4.3 Art Assets
The art assets for the game will be Two-Dimensional and will be either free use from the web or generated by the team. We do not anticipate any issue with developing the assets required for the game.

## 3.5 Requirements Apportioning
<div id = "PriorityTable" style="width:35%; border:2px solid">

| Priority Level | Description |
|:--------------:|:-----------:|
| 1 | **Priority 1** requirements are essential to the correct functionality of the system. These requirements must be extensively tested and verified to ensure the system fundamentally works. |
| 2 | **Priority 2** requirements are non-essential, but may be considered in the design. These requirements are not guaranteed to be in the final build of the system. The system should function as expected without fulfilling these requirements. If all requirements of priority 1 are fulfilled, the team will consider fulfilling requirements of priority 2. |
| 3 | **Priority 3** requirements are non-essential and will not be considered in the design. These requirements are not likely to be in the final build of the system. The system should function as expected without fulfilling these requirements. If all requirements of priority 1 and 2 are fulfilled, the team will consider fulfilling requirements of priority 3.

</div>
