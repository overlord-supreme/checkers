using System;
using System.Collections.Generic;

/// <summary>
/// Represents allowed diagonal movements of length 2 in which a piece jumps over another piece and captures it.
/// </summary>
public class JumpMoveRule : MoveRule
{
    public JumpMoveRule(Board board) : base(board) {}

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
            DELTAS.Add(Tuple.Create( 2, 2));
            DELTAS.Add(Tuple.Create(-2, 2));
        }

        // Add front two positions if piece is black (or kinged).
        if (isKinged || (color.HasValue && color.Value == Piece.PieceColor.BLACK))
        {
            DELTAS.Add(Tuple.Create( 2,-2));
            DELTAS.Add(Tuple.Create(-2,-2));
        }

        foreach (Tuple<int, int> delta in DELTAS) {
            int x0 = startPos.Item1 + (delta.Item1 / 2);
            int y0 = startPos.Item2 + (delta.Item2 / 2);

            Space jumpOver = board.GetSpaceByLoc(x0, y0);

            int x1 = startPos.Item1 + delta.Item1;
            int y1 = startPos.Item2 + delta.Item2;

            Space jumpTarget = board.GetSpaceByLoc(x1, y1);

            // Highlight spaces that are 1 length away on the diagonal if the player can jump.
            bool isOpponentPiece = jumpOver != null && jumpOver.isOccupied() && jumpOver.currentOccupantIsColor(color.Value == Piece.PieceColor.BLACK ? Piece.PieceColor.BLACK : Piece.PieceColor.RED);
            if (isOpponentPiece && jumpTarget != null && !jumpTarget.isOccupied())
                yield return jumpTarget;
        }
    }
}