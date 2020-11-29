using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Data for a single Tile/Cell/Space/Square on the Board
/// </summary>
[Serializable]
public class Space : MonoBehaviour
{
    
    // LOGICAL position of TILE
    public int x = 0;
    public int y = 0;

    // PHYSICAL location of PIECE SPAWN POINT
    public Transform piecePosition;
    
    // Stores a reference to the current occupant of itself
    [SerializeField] private Piece currentOccupant = null;

    // The highlight for possible moves.
    [SerializeField] private GameObject TileHighlight;

    // Whether the current space is the "edge" of the board for king check
    private bool isEdge = false;

    // Whether the current space represents a valid move for the player.
    private bool highlighted = false;
    
    // Used for edge, color of side
    [SerializeField] private Piece.PieceColor color = Piece.PieceColor.NONE;
    
    // returns bool for if the space is currently occupied
    public bool isOccupied() {return currentOccupant != null;}

    private void Start()
    {
        TileHighlight.SetActive(false);
    }

    /// <summary>
    /// returns reference to current occupant
    /// </summary>
    public Piece getCurrentOccupant() {return currentOccupant;}
    

    /// <summary>
    /// removes held piece
    /// </summary>
    public void clearCurrentOccupant() {currentOccupant = null;}
    

    /// <summary>
    /// sets held piece
    /// </summary>
    public void setCurrentOccupant(Piece occupant) {currentOccupant = occupant; currentOccupant.transform.position = piecePosition.position;}
    

    /// <summary>
    /// sets edge
    /// </summary>
    public void setEdge(bool isEdge) {this.isEdge = isEdge;}
    

    /// <summary>
    /// sets color
    /// </summary>
    public void setColor(Piece.PieceColor color) {this.color = color;}

    /// <summary>
    /// Determines whether this piece is visible to the player.
    /// </summary>
    public bool isHighlighted() { return highlighted; }

    /// <summary>
    /// Shows or hides the highlight.
    /// </summary>
    public void setHighlighted(bool shouldBeVisible) {
        TileHighlight.SetActive(shouldBeVisible);
        highlighted = shouldBeVisible;
    }

    /// <summary>
    /// Convenience method for checking whether the piece on the space matches a specific color.
    /// </summary>
    public bool currentOccupantIsColor(Piece.PieceColor pieceColor) {
        return isOccupied() && currentOccupant.color == pieceColor;
    }
}
