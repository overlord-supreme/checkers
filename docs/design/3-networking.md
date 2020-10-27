# 3 Networking Design Overview

The figure below shows the UML diagram of our networking interface.

```mermaid
classDiagram
    class GameManager{
        +GameObject winnerUI
        -GameObject board

        -Start() void
        -Update() void
        +OnPlayerLeftRoom(Player Other) void
        +QuitRoom() void
    }
    GameManager <-- Launcher
    GameManager --> BoardManager
    class Launcher{
        -GameObject controlPanel;
        -Text feedbackText;
        -byte maxPlayersPerRoom = 2;
        -bool isConnecting;
        -string gameVersion = "1";
        +InputField playerNameField;
        +InputField roomNameField;
        +Text playerStatus;
        +Text connectionStatus;
        +GameObject roomJoinUI;
        +GameObject buttonLoadArena;
        +GameObject buttonJoinRoom;
        -string playerName = "";
        -string roomName = "";

        -Start() void
        -Awake() void
        +SetPlayerName(string name) void 
        +SetRoomName(string name) void 
        +ConnectToPhoton() void 
        +JoinRoom() void 
        +LoadArena() void
        +OnConnected() void
        +OnDisconnected(DisconnectCause cause) void
        +OnJoinedRoom() void
    }
    
    class BoardManager{
        -enum CurrentPlayer
        -Start() void
        -GenerateBoard() void
        -StartTurn() void
        -MovePiece(Piece piece) bool
        -EvaluateMoves(Piece piece) void
        -EndTurn() void
        -ClearMoves() void
        -EvaluateWinLoss() bool
    }
```

Referenced this for network structure: https://www.raywenderlich.com/1142814-introduction-to-multiplayer-games-with-unity-and-photon