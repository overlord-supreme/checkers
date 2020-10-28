# 1 Introduction

### 1.1 Purpose

The purpose of this design document to provide design guidelines for the
implementation of the Overlord-Supreme Checkers game. This document will serve
as a reference for the developers coding the game.

### 1.2 Scope

The major functionality of Overloard-Supreme Checkers is to provide an
interface for two players to compete in an online match of checkers.

### 1.3 Definitions

#### 1.3.1 Checkers

**`Tile`** A spot on the board upon which a piece could be placed or moved.

**`Man`** A regular checkers piece.

**`King`** A piece that can move diagonally backward.

**`Player`** A user who has connected to an opponent in the game and is about
to begin the game, in the process of playing the game, or has finished the
game. A player either controls the light or dark pieces. This term will be
used to refer to the current player (the player whose turn it is).

**`Opponent`** A user who has connected to a game against the player. The
opponent is also a player.

**`Move`** An action either player can take. This action involves moving a
piece from one tile to another at least once, and may involve capturing
another tile.

**`Turn`** A time frame in which either player makes one or more moves.

**`Pile`** A player's collection of captured opponent pieces.

#### Software

**`Photon Unity Networking`** or **`PUN`** A C# library for creating server-
based networking applications.


