using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Space : MonoBehaviour
{
    //Transform for piece position
    public Transform piecePosition;
    //Stores a reference to the current occupant of itself
    private Piece currentOccupant = null;
    //Whether the current space is the "edge" of the board for king check
    private bool isEdge = false;
    //Used for edge, color of side
    [SerializeField] private Piece.PieceColor color = Piece.PieceColor.NONE;
    //returns bool for if the space is currently occupied
    public bool isOccupied(){return currentOccupant != null;}
    //returns reference to current occupant
    public Piece getCurrentOccupant(){return currentOccupant;}
    //sets edge
    public void setEdge(bool isEdge) {this.isEdge = isEdge;}
    //sets color
    public void setColor(Piece.PieceColor color) {this.color = color;}
}
