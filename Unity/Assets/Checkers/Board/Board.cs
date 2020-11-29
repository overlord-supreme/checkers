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
    [SerializeField] private GameObject RedPiecePrefab;
    [SerializeField] private GameObject BlackPiecePrefab;
    [SerializeField] private GameObject PiecesList;


    // How we Track Logical Position
    private Space[,] boardGrid = new Space[8,8];


    // Totally a hack, no "gameObject" in "MonoBehaviourPunCallbacks"
    // Publicly set from the Inspector
    public GameObject gameObject;


    // Player Management
    public enum PlayerColor {BLACK, RED, NONE};


    // Event Codes
    private const byte pieceMoveCode = 1;
    private const byte piecePromoteCode = 2;
    private const byte pieceDestroyCode = 3;
    private const byte playerSwapCode = 4;


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

        for(int i = 0; i < 3; i++)
        {
            int secondHalf = 8 - i - 1; 
            for(int g = 0; g < 8; g++)
            {
                if(i % 2 == 0) // row logic
                {
                    if(g % 2 == 0) // column logic
                    {
                        GameObject piece = GameObject.Instantiate(BlackPiecePrefab,PiecesList.transform);
                        boardGrid[g,secondHalf].setCurrentOccupant(piece.GetComponent<Piece>());
                    }
                    else
                    {
                        GameObject pieceObj = GameObject.Instantiate(RedPiecePrefab,PiecesList.transform);
                        Piece piece = pieceObj.GetComponent<Piece>();
                        boardGrid[g,i].setCurrentOccupant(piece);
                        piece.color = Piece.PieceColor.RED;
                    }
                }
                else
                {
                    if(g % 2 == 1)
                    {
                        GameObject piece = GameObject.Instantiate(BlackPiecePrefab,PiecesList.transform);
                        boardGrid[g,secondHalf].setCurrentOccupant(piece.GetComponent<Piece>());
                    }
                    else
                    {
                        GameObject pieceObj = GameObject.Instantiate(RedPiecePrefab,PiecesList.transform);
                        Piece piece = pieceObj.GetComponent<Piece>();
                        boardGrid[g,i].setCurrentOccupant(piece);
                        piece.color = Piece.PieceColor.RED;
                    }
                }
            }
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

        object[] data = (object[])photonEvent.CustomData;
        // If someone wants to destroy a piece
        if (eventCode == pieceDestroyCode)
        {
            DestroyPiece((int[])photonEvent.Parameters[pieceDestroyCode]);
        }

        // If someone wants to move a piece
        if (eventCode == pieceMoveCode)
        {
            MovePiece((int)data[0],(int)data[1],(int)data[2],(int)data[3]);
        }

        // If someone wants to promote a piece
        if (eventCode == piecePromoteCode)
        {
            //
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
    private void MovePiece(int startX, int startY, int endX, int endY)
    {
        // First two elements of the array are the START POSITION
        
        // Find the Piece
        Piece piece = GetPieceByLoc(startX,startY);
        
        // Remove Occupants from the START LOCATION
        GetSpaceByLoc(startX,startY).clearCurrentOccupant();
        
        // Move the Found Piece to the END LOCATION
        GetSpaceByLoc(endX,endY).setCurrentOccupant(piece);
    }


    /// <summary>
    /// LOGICAL position on board
    /// </summary>
    public Piece GetPieceByLoc(int x, int y)
    {
        return boardGrid[x,y].getCurrentOccupant();
    }


    /// <summary>
    /// LOGICAL position on board
    /// </summary>
    public Space GetSpaceByLoc(int x, int y)
    {
        return boardGrid[x,y];
    }


    /// <summary>
    /// Gets the logical position on board of the space.
    /// Returns null if the space is not on the board.
    /// </summary>
    public Tuple<int, int> GetLocBySpace(Space space)
    {
        for (int i = 0; i < boardGrid.GetLength(0); i++) {
            for (int j = 0; j < boardGrid.GetLength(1); j++) {
                if (boardGrid[i, j].Equals(space))
                    return new Tuple<int, int>(i, j);
            }
        }
        return null;
    }


    /// <summary>
    /// Gets the logical position on board of the piece.
    /// Returns null if the piece is not on the board.
    /// </summary>
    public Tuple<int, int> GetLocByPiece(Piece piece)
    {
        for (int i = 0; i < boardGrid.GetLength(0); i++) {
            for (int j = 0; j < boardGrid.GetLength(1); j++) {
                Piece mbOccupant = boardGrid[i, j].getCurrentOccupant();
                if (mbOccupant != null && mbOccupant.Equals(piece))
                    return new Tuple<int, int>(i, j);
            }
        }
        return null;
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
    public void RequestMove(int startX, int startY, int endX, int endY)
    {
        object[] content = new object[] { startX, startY, endX, endY };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions{Receivers = ReceiverGroup.All};
        PhotonNetwork.RaiseEvent(pieceMoveCode, content, raiseEventOptions, SendOptions.SendReliable);
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

        IEnumerable<Space> possibleMoves = AvailableMoves(targetSpace);
        
        // Highlight each available move for the current player
        foreach (Space move in possibleMoves)
            move.setHighlighted(true);
        
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
    /// Yields each available move the player can make from some starting position.
    /// </summary>
    private IEnumerable<Space> AvailableMoves(Space start)
    {
        IEnumerable<MoveRule> moves = new List<MoveRule>()
        {
            new BasicMoveRule(this),
            new JumpMoveRule(this)
        };

        foreach (MoveRule move in moves)
        {
            foreach (Space space in move.GetMoves(start))
            {
                yield return space;
            }
        }
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
        // Unhighlight all spaces no matter what the user does
        for (int i = 0; i < boardGrid.GetLength(0); i++)
            for (int j = 0; j < boardGrid.GetLength(1); j++)
                boardGrid[i,j]?.setHighlighted(false);

        // Target space represents an available move.
        if (targetSpace.isHighlighted())
        {
            // REPLACE ME WITH NICE SIDE EFFECTS!!!
            Debug.Log("RequestSecondClick: EMPTY, SELECT ME");
            return true;
        }
        
        // Anything Else
        // REPLACE ME WITH NICE SIDE EFFECTS!!!
        Debug.Log("RequestSecondClick: NOT empty, DO NOT select me");
        return false;
    }
}