
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

### 4.? Matchmaking

### 4.? Environment

#### `R?.?` Checkers Board
- **`R?.?.?`** The board shall consist of 64 tiles in an 8x8 layout.
- **`R?.?.?`** The board shall have 32 dark and 32 light tiles in an
               alternating pattern.
- **`R?.?.?`** The bottom right-most tile shall be a light tile for both
               players.
- **`R?.?.?`** The board shall have 12 men of each color at start of game.

#### `R?.?` Players
- **`R?.?.?`** A match shall consist of two players.
- **`R?.?.?`** One player shall control the dark pieces,
               and the other shall control the light pieces.

### 4.? Start of Game

#### `R?.?` Piece Placement
- **`R?.?.?`** At start of game, the men for each player will be placed on
               the dark tiles in the first three rows of their respective
               sides.
- **`R?.?.?`** The middle two rows shall remain empty until a player makes a
               move.

#### `R?.?` Match Initialization
- **`R?.?.?`** A player will randomly be chosen to take the first turn.

### 4.? Gameplay

#### `R?.?` Taking Turns
- **`R?.?.?`** Turns are in alternating order.
- **`R?.?.?`** A player shall not be able to choose to pass if there is a move
               available.
- **`R?.?.?`** If a player fails to take a turn after a long amount of time,
               they shall pass their turn.
- **`R?.?.?`** If all of a player's available moves are blocked, they shall
               pass their turn without making any moves.

#### `R?.?` Piece Movement
- **`R?.?.?`** A man shall always be moved diagonally forward.
- **`R?.?.?`** A king shall be able to be moved diagonally backward or
               forward.
- **`R?.?.?`** A piece shall either be moved one tile into an empty tile, or 
               capture an opposing piece.
- **`R?.?.?`** A piece can be moved again on the same turn if it captured a
               piece. This can continue until there are no pieces left to
               capture or the player chooses to end their turn.

#### `R?.?` Capturing
- **`R?.?.?`** The following criteria shall be fulfilled for a capture to take
               place:
  * The capturing piece must be one tile away on the diagonal from an opponent
    piece (the captured piece). If the capturing piece is a king, the captured
    piece can be behind the king.
  * The player must be able to move the capturing piece two tiles diagonally
    such that it ends up on the opposite side of the captured piece.
  * There must be an empty tile on the opposite side of the captured piece.
- **`R?.?.?`** After capturing once, the capturing piece can continue
               capturing other pieces. The capturing piece shall be able to
               change direction while capturing multiple pieces.
- **`R?.?.?`** After a capture, only the capturing pieces may be moved again.
- **`R?.?.?`** Any captured pieces shall be removed from the board at the
               end of the turn and be added to the player's pile.
- **`R?.?.?`** Since captured pieces remain until the end of the turn, a king
               shall be able to capture a piece twice. A man shall not be able
               to do this, since it can only move forward. Capturing a piece
               twice gives no extra scoring benefit.

#### `R?.?` Kinging
- **`R?.?.?`** A man that has reached the opponents first row of the board
               becomes a king on that same turn. 
- **`R?.?.?`** A king shall be able to move diagonally backward.

### 4.? End of Match
- **`R?.?.?`** A match shall end when a player has no remaining pieces on the
               board.
- **`R?.?.?`** At the end of a match, the player with pieces remaining on the
               board is the winner.
- **`R?.?.?`** The player that is not the winner loses the match.
- **`R?.?.?`** Either player shall be able to choose to leave the match at
               any point during the match. This player is the loser, and the
               opposing player wins the match.

