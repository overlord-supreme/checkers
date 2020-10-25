# 2 Design Overview

## 2.1 Description of Problem

The problem is creating a way to play American checkers over the internet with automatic rules enforcement. 

## 2.2 Technologies Used

This checkers game will use Unity3D as the core engine for rendering models and taking in user input. We will be utilizing Photon's PUN unity asset in order to handle the networking for the game.

The target platform will be Microsoft Windows and Linux, and the development environment will be Visual Studio Code and IntelliJ.

## 2.3 System Architecture

This system will be constructed from the following components:
* Photon PUN - A networking cloud service that integrates with the Unity engine through a marketplace asset and is used to handle data synchronization between clients. 
* Game Interface - The UI that the player will interact with to play checkers.
* Game Model - All of the classes related to creating our checkers game, such as the Board, Piece, Player, etc. All of the game data during a checkers game is stored in-memory inside of the game model and updated/synchronized using Photon PUN.
* Local Storage - Storage for data across sessions, like names. 

```mermaid
graph TD;
    id1((Player)) --> |User Input|Game_Interface;
    Game_Interface --> |Interaction With Game|Game_Model;
    Local_Storage --> |Load Name|Game_Interface
    Game_Model --> |Update Visuals|Game_Interface
    Game_Interface --> |Save Name|Local_Storage;
    Local_Storage --> |Set Name|Game_Model;
    Game_Model --> |Send Player Data|Photon_PUN
    Photon_PUN --> |Recieve Opponent Data|Game_Model
    Photon_PUN --> id2((Opponent))
    id2((Opponent)) --> Photon_PUN
```
*Figure 1* above shows the connections between our high-level components.

## 2.4 System Operation

*Figure 2* shows the sequence of events that occur during a normal game of checkers.

```mermaid
sequenceDiagram
    Player -->>Game Interface: User interaction
    Game Interface -->> Local Storage: Create/Load name
    Local Storage -->> Game Model: Name is loaded
    Game Model -->> Game Interface: Name is updated on display
    Game Interface -->> Game Model: Create Lobby
    Game Model -->> Photon PUN: Creates new lobby on network with player's data
    Photon PUN -->> Game Model: Opponent Connects to lobby
    Game Model -->> Game Interface: Update Names on Display
    Game Interface -->> Game Model: Start Game
    Game Model -->> Photon PUN: Sync Start Game
    loop Game Loop Until Game Is Over
    Player -->> Game Interface: Select Move
    Game Interface -->> Game Model: Update Board with User Move
    Game Model -->> Photon PUN: Send Player Move
    Photon PUN -->> Game Model: Recieve Opponent Move
    Game Model -->> Game Interface: Shows end screen if game is over, else updates board
    end
    Player -->> Game Interface: User clicks on exit to menu
    Game Interface -->> Game Model: Exit to Menu and disconnect from server
    Game Model -->> Game Interface: Update screen to show menu
    Player -->> Game Interface: User closes Application or creates/joins new Lobby
    Game Interface -->> Game Model: Close application, or start new game
    
```
