using System.Collections.Generic;

/// <summary>
/// Represents an abstract set of allowed movement rules based on a board and a starting space.
/// </summary>
public abstract class MoveRule
{

    /// <summary>
    /// The board to determine whether moves are allowed.
    /// </summary>
    protected Board board { get; }

    /// <summary>
    /// Instantiantes a new rule checker with the given board.
    /// </summary>
    protected MoveRule(Board board)
    {
        this.board = board;
    }

    /// <summary>
    /// Provides an enumerable set of spaces which are allowed from the given start space.
    /// </summary>
    public abstract IEnumerable<Space> GetMoves(Space start);
}