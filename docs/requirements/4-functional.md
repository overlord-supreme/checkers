
# 4. Functional Requirements

### 4.0 Definitions

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

**`Turn`** A time frame in which either the player or the opponent makes one
or more moves.

**`Pile`** A player's collection of captured opponent pieces.

### 4.1 Matchmaking

#### `R1.1` Player Identification
- **`R1.1.1`** Upon starting the game, the player shall be prompted to enter
               a username.
               **Priority 1**
- **`R1.1.1`** The player's username shall persist across game sessions.
               **Priority 3**

#### `R1.2` Menu
- **`R1.2.1`** After entering a username, the player shall be directed to a
               menu.
               **Priority 1**
- **`R1.2.2`** On the menu, the player shall be able to create a lobby or join
               an existing lobby.
               **Priority 1**
- **`R1.2.3`** On the menu, the player shall be able to exit the game.
               **Priority 1**

#### `R1.3` Lobbies
- **`R1.3.1`** A lobby shall be identified by the player who started it.
               **Priority 1**
- **`R1.3.2`** A lobby shall be able to be private (password-protected) or
               public.
               **Priority 4**
- **`R1.3.3`** Once two players are in the lobby, both players shall need to
               "ready-up" before the match is started.
               **Priority 2**
- **`R1.3.4`** After a match is completed, both players shall return to the
               lobby.
               **Priority 2**

### 4.2 Environment

#### `R2.1` Checkers Board
- **`R2.1.1`** The board shall consist of 64 tiles in an 8x8 layout.
               **Priority 1**
- **`R2.1.2`** The board shall have 32 dark and 32 light tiles in an
               alternating pattern.
               **Priority 1**
- **`R2.1.3`** The bottom right-most tile shall be a light tile for both
               players.
               **Priority 1**
- **`R2.1.4`** The board shall have 12 men of each color at start of game.
               **Priority 1**

#### `R2.2` Players
- **`R2.2.1`** A match shall consist of two players.
               **Priority 1**
- **`R2.2.2`** One player shall control the dark pieces,
               and the other shall control the light pieces.
               **Priority 1**

### 4.3 Start of Game

#### `R3.1` Piece Placement
- **`R3.1.1`** At start of game, the men for each player will be placed on
               the dark tiles in the first three rows of their respective
               sides.
               **Priority 1**
- **`R3.1.2`** The middle two rows shall remain empty until a player makes a
               move.
               **Priority 1**

#### `R3.2` Match Initialization
- **`R3.2.1`** A player shall randomly be chosen to take the first turn.
               **Priority 2**

### 4.4 Gameplay

#### `R4.1` Taking Turns
- **`R4.1.1`** Turns shall be in alternating order.
               **Priority 1**
- **`R4.1.2`** A player shall not be able to choose to pass if there is a move
               available.
               **Priority 2**
- **`R4.1.3`** If a player fails to take a turn after a long amount of time,
               they shall pass their turn.
               **Priority 3**
- **`R4.1.4`** If all of a player's available moves are blocked, they shall
               pass their turn without making any moves.
               **Priority 3**

#### `R4.2` Piece Movement
- **`R4.2.1`** A man shall always be moved diagonally forward.
               **Priority 1**
- **`R4.2.2`** A king shall be able to be moved diagonally backward or
               forward.
               **Priority 1**
- **`R4.2.3`** A piece shall either be moved one tile into an empty tile, or 
               capture an opposing piece.
               **Priority 1**
- **`R4.2.4`** A piece can be moved again on the same turn if it captured a
               piece. This can continue until there are no pieces left to
               capture or the player chooses to end their turn.
               **Priority 2**

#### `R4.3` Capturing
- **`R4.3.1`** The following criteria shall be fulfilled for a capture to take
               place:
               **Priority 1**
  * The capturing piece must be one tile away on the diagonal from an opponent
    piece (the captured piece). If the capturing piece is a king, the captured
    piece can be behind the king.
  * The player must be able to move the capturing piece two tiles diagonally
    such that it ends up on the opposite side of the captured piece.
  * There must be an empty tile on the opposite side of the captured piece.
- **`R4.3.2`** After capturing once, the capturing piece can continue
               capturing other pieces. The capturing piece shall be able to
               change direction while capturing multiple pieces.
               **Priority 2**
- **`R4.3.3`** After a capture, only the capturing pieces may be moved again.
               **Priority 1**
- **`R4.3.4`** Any captured pieces shall be removed from the board at the
               end of the turn and be added to the player's pile.
               **Priority 1**
- **`R4.3.5`** Since captured pieces remain until the end of the turn, a king
               shall be able to capture a piece twice. A man shall not be able
               to do this, since it can only move forward. Capturing a piece
               twice gives no extra scoring benefit.
               **Priority 3**

#### `R4.4` Kinging
- **`R4.4.1`** A man that has reached the opponents first row of the board
               becomes a king on that same turn. 
               **Priority 1**
- **`R4.4.2`** A king shall be able to move diagonally backward.
               **Priority 1**

### 4.5 End of Match
- **`R4.5.1`** A match shall end when a player has no remaining pieces on the
               board.
               **Priority 1**
- **`R4.5.2`** At the end of a match, the player with pieces remaining on the
               board is the winner.
               **Priority 1**
- **`R4.5.3`** The player that is not the winner loses the match.
               **Priority 1**
- **`R4.5.4`** Either player shall be able to choose to leave the match at
               any point during the match. This player is the loser, and the
               opposing player wins the match.
               **Priority 2**

