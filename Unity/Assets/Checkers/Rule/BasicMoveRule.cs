using System;
using System.Collections.Generic;

/// <summary>
/// Represents allowed diagonal movements of length 1 (i.e. no jumps).
/// </summary>
public class BasicMoveRule : MoveRule
{
    public BasicMoveRule(Board board) : base(board) {}

    public override IEnumerable<Space> GetMoves(Space start)
    {
        Piece startPiece = start.getCurrentOccupant();
        Piece.PieceColor? color = startPiece.color;
        bool isKinged = startPiece != null && startPiece.kinged;
        Tuple<int, int> startPos = board.GetLocBySpace(start);

        List<Tuple<int, int>> DELTAS = new List<Tuple<int, int>>();

        // Add back two positions if piece is red (or kinged).
        if (isKinged || (color.HasValue && color.Value == Piece.PieceColor.RED))
        {
            DELTAS.Add(Tuple.Create( 1, 1));
            DELTAS.Add(Tuple.Create(-1, 1));
        }

        // Add front two positions if piece is black (or kinged).
        if (isKinged || (color.HasValue && color.Value == Piece.PieceColor.BLACK))
        {
            DELTAS.Add(Tuple.Create( 1,-1));
            DELTAS.Add(Tuple.Create(-1,-1));
        }

        foreach (Tuple<int, int> delta in DELTAS) {
            int x = startPos.Item1 + delta.Item1;
            int y = startPos.Item2 + delta.Item2;

            Space jumpOver = board.GetSpaceByLoc(x, y);

            // Highlight adjacent spaces if they are empty.
            if (jumpOver != null && !jumpOver.isOccupied())
                yield return jumpOver;
        }
    }
}