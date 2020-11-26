using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;


/// <summary>
/// Responsibilities:
///     Local Piece Management
///     Rule Adjudication
///     Network Packet Decoding
///     Network Packet Reading
///     Network Packet Writing
/// Decisions:
///     Event Methods assume the Requester is acting in good faith
///     Hence, no Networked Methods are to be rejected
///     Very NAIVE, yes, but avoids negotiation protocols
/// </summary>
/// <see href="https://doc.photonengine.com/en-us/pun/v1/demos-and-tutorials/package-demos/rockpaperscissors-demo">RPCs and RaiseEvent | Photon Engine</see>
public class Board : MonoBehaviourPunCallbacks, IOnEventCallback
{

    // Piece Management (LOCAL)
    [SerializeField] private GameObject WhitePiecePrefab;
    [SerializeField] private GameObject BlackPiecePrefab;
    [SerializeField] private GameObject PiecesList;


    // How we Track Logical Position
    private Space[,] boardGrid = new Space[8,8];


    // Totally a hack, no "gameObject" in "MonoBehaviourPunCallbacks"
    // Publicly set from the Inspector
    public GameObject gameObject;


    // Player Management
    public enum PlayerColor {BLACK, WHITE, NONE};


    // Event Codes
    private const byte pieceMoveCode = 1;
    private const byte piecePromoteCode = 2;
    private const byte pieceDestroyCode = 3;
    private const byte playerSwapCode = 4;


    // Parameter Codes
    private const byte pieceMoveData = 60;
    private const byte piecePromoteData = 61;
    private const byte pieceDestroyData = 62;
    private const byte playerSwapData = 63;


    // Board MUST BE SINGLETON
    private static Board singleton = null;
    public static Board getInstance() {return singleton;}


    /// <summary>
    /// Sets up Board with Single Reference and Logical Positions
    /// </summary>
    private void Start() 
    {
        // Ensure this is the only Board in Existence
        if (singleton == null)
            singleton = this;
        else
            GameObject.Destroy(this.gameObject);
        
        // Load the Tile LOGICAL POSITIIONS into our 2D LOOKUP array
        Space[] spaces = gameObject.GetComponentsInChildren<Space>();
        foreach(Space space in spaces)
        {
            boardGrid[space.x,space.y] = space;
        }
    }


    /// <summary>
    /// Begin listening to the network
    /// </summary>
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }


    /// <summary>
    /// Stop listening to the network
    /// </summary>
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    /// <summary>
    /// Switches on event code
    /// </summary>
    /// <see href="https://doc.photonengine.com/en-us/pun/current/gameplay/rpcsandraiseevent#ioneventcallback_callback">IOnEventCallback Callback</see>
    public void OnEvent(EventData photonEvent)
    {
        // Extract the Event Code (to switch on)
        byte eventCode = photonEvent.Code;


        // If someone wants to destroy a piece
        if (eventCode == pieceDestroyCode)
        {
            DestroyPiece((int[])photonEvent.Parameters[pieceDestroyCode]);
        }

        // If someone wants to move a piece
        if (eventCode == pieceMoveCode)
        {
            MovePiece((int[])photonEvent.Parameters[pieceMoveData]);
        }

        // If someone wants to promote a piece
        if (eventCode == piecePromoteCode)
        {
            PromotePiece((int[]) photonEvent.Parameters[piecePromoteData]);
        }

        // 2020-11-25T19:21:00-05: Not Used, Still WIP
        // if (eventCode == playerSwapCode)
        // {
        //     SwapPlayer((int)photonEvent.Parameters[playerSwapData]);
        // }
    }


    /// <summary>
    /// Replace a King Piece by LOGICAL POSITION
    /// </summary>
    private void PromotePiece(int[] logicalPositions)
    {
        // Find the Location
        int x = logicalPositions[0];
        int y = logicalPositions[1];
    }
 

    /// <summary>
    /// Destroys a Piece by LOGICAL POSITION
    /// </summary>
    private void DestroyPiece(int[] logicalPositions)
    {
        // Find the Location
        int x = logicalPositions[0];
        int y = logicalPositions[1];

        // Get the Space and Clear it
        GetSpaceByLoc(x,y).clearCurrentOccupant();

        // Get the Piece and Delete it
        // gameObject != GameObject
        GameObject.Destroy(GetPieceByLoc(x,y));
    }


    /// <summary>
    /// Uses LOGICAL POSITION
    /// MOVES the piece at START LOCATION to END LOCATION
    /// This is called from the NETWORK, not from the PLAYER
    /// It is NOT OK for this method to FAIL
    /// (Disagreement on the Network is not good)
    /// </summary>
    private void MovePiece(int[] logicalPositions)
    {
        // First two elements of the array are the START POSITION
        int firstX = logicalPositions[0];
        int firstY = logicalPositions[1];
        
        // Last two are the END POSITION
        int secondX = logicalPositions[2];
        int secondY = logicalPositions[3];
        
        // Find the Piece
        Piece piece = GetPieceByLoc(firstX,firstY);
        
        // Remove Occupants from the START LOCATION
        GetSpaceByLoc(firstX,firstY).clearCurrentOccupant();
        
        // Move the Found Piece to the END LOCATION
        GetSpaceByLoc(secondX,secondY).setCurrentOccupant(piece);
    }


    /// <summary>
    /// LOGICAL position on board
    /// </summary>
    private Piece GetPieceByLoc(int x, int y)
    {
        return boardGrid[x,y].getCurrentOccupant();
    }


    /// <summary>
    /// LOGICAL position on board
    /// </summary>
    private Space GetSpaceByLoc(int x, int y)
    {
        return boardGrid[x,y];
    }


    /// <summary>
    /// Swaps who is the ACTIVE player
    /// Uses INT to indicate next Player
    /// NOTICE, not in use right now, WIP, FIXME !!!
    /// </summary>
    private void SwapPlayer(int nextPlayer)
    {
        // Player 1's Turn
        // if (nextPlayer == players.player1)
        // {

        // }
        
        // // Player 2's Turn
        // if (nextPlayer == players.player2)
        // {

        // }
    }





    /// <summary>
    /// This takes in a Player's request
    /// CHECKS to see if it is legal
    /// And if so, makes a MovePiece REQUEST on the network
    /// It is OK for this method to FAIL
    /// This checks to see if the PATH is valid
    /// DELEGATES to <see cref="Board.RequestFirstClick" /> and <see cref="Board.RequestSecondClick" /> and <see cref="Board.GetOptions" />
    /// Runs AFTER the delegates, now does algebar on the PATH ITSELF to see if it is valid
    /// Checks EACH of the Cardinal Diagonals (NE, NW, SW, SE)
    /// QUESTION, do we want to allow greater than 1 space hops? (ie, 2, 3, etc)
    /// </summary>
    public void RequestMove(int[] start, int[] end)
    {
        
        // FIXME
    }




    /// <summary>
    /// EXPECTS <see cref="PlayerManager" /> to call this method
    /// Upon a user mouse click
    /// We DO NOT expect EVERY kind of GameObject
    /// We Expect ONLY Tile GameObjects (Locked by mask)
    /// And return what the user can do with it as SIDE EFFECTS
    /// Checks to see if the START LOCATION is valid
    /// Part of <see cref="RequestMove" />
    /// Empty/Enemy Space = Highlight with "Not Selected"
    /// Friendly Space = Highlight with "Selected"
    /// Manages Side Effects
    /// Manages Validation
    /// NO Options, see: <see cref="GetOptions" />
    /// Returns Space Occupant IF the user can select this space
    /// Returns NULL if the user CANNOT select this space
    /// </summary>
    public Piece RequestFirstClick(Space targetSpace, Piece.PieceColor playerColor)
    {
        // Empty
        if (!targetSpace.isOccupied())
        {
            // REPLACE ME WITH NICE SIDE EFFECTS!!!
            Debug.Log("RequestFirstClick: EMPTY, DO NOT select me");
            return null;
        }
        
        // Get the piece
        Piece piece;
        piece = targetSpace.getCurrentOccupant();
        // TODO FOR CHRIS -- If a piece is on the tile, now highlight the tile and the potential move locations
        
        // Friendly
        if (piece.color == playerColor)
        {
            // REPLACE ME WITH NICE SIDE EFFECTS!!!
            Debug.Log("RequestFirstClick: FRIENDLY, SELECT ME");
            return piece;
        }
        
        // Anything Else
        // REPLACE ME WITH NICE SIDE EFFECTS!!!
        Debug.Log("RequestFirstClick: ENEMY, DO NOT select me");
        return null;
    }





    /// <summary>
    /// EXPECTS <see cref="PlayerManager" /> to call this method
    /// Upon a user mouse click *following* a successful mouse click
    /// Checks to see if the END LOCATION is valid
    /// Part of <see cref="RequestMove" />
    /// Manages Side Effects
    /// Manages Validation
    /// NO Options, see: <see cref="GetOptions" />
    /// Returns TRUE IF the user can select this space
    /// Returns FALSE if the user CANNOT select this space
    /// </summary>
    public bool RequestSecondClick(Space targetSpace)
    {   
        // Empty
        if (!targetSpace.isOccupied())
        {
            // REPLACE ME WITH NICE SIDE EFFECTS!!!
            Debug.Log("RequestFirstClick: EMPTY, SELECT ME");
            return true;
        }
        
        // Anything Else
        // REPLACE ME WITH NICE SIDE EFFECTS!!!
        Debug.Log("RequestFirstClick: NOT empty, DO NOT select me");
        return false;
    }
    

    /// <summary>
    /// Could return EMPTY
    /// Or Return ANY in Cardinal Diagonals
    /// Expects <see cref="PlayerManager" /> to call this method
    /// ASSUMES both <see cref="Board.RequestFirstClick" /> and <see cref="Board.RequestSecondClick" /> have run successfully
    /// FIXME NOT IMPLEMENTED !!!
    /// </summary>
    // public ArrayList<Space> GetOptions(int playerColor)
    // {

    // }


}