using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BoardSpace
{
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

        // The highlight that tells us whether a player can make a move. Hidden by default.
        [SerializeField] private GameObject TileHighlight;

        // Used for edge, color of side
        [SerializeField] private Piece.PieceColor color = Piece.PieceColor.NONE;

        // returns bool for if the space is currently occupied
        public bool isOccupied() { return currentOccupant != null; }

        private void Start()
        {
            TileHighlight.SetActive(false);
        }

        /// <summary>
        /// returns reference to current occupant
        /// </summary>
        public Piece getCurrentOccupant() { return currentOccupant; }


        /// <summary>
        /// removes held piece
        /// </summary>
        public void clearCurrentOccupant() { currentOccupant = null; }


        /// <summary>
        /// sets held piece
        /// </summary>
        public void setCurrentOccupant(Piece occupant) { 
            currentOccupant = occupant;
            currentOccupant.transform.position = piecePosition.position;
        }

        /// <summary>
        /// sets whether the highlight is visible
        /// </summary>
        public void setHighlighted(bool isHighlighted) { TileHighlight.SetActive(isHighlighted); }
    }
}
