using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using Space = BoardSpace.Space;

/// <summary>
/// Stores the TARGET space and the LOGICAL POSITION of the jumped piece
/// Generated FROM a particular space (not included in the struct)
/// ((The START POSITION is IMPLIED, and not listed explicitly))
/// WARN jumped can be EMPTY
/// Jumped should only be ZERO elements (No Positions) or TWO elements (X, Y)
/// </summary>
public struct ValidMove
{
    public Piece piece;
    public Space targetSpace;
    public int[] jumped;
    public bool isJump;
}


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
    public Material selectableMaterial;

    // How we Track Logical Position
    public Space[,] boardGrid = new Space[8,8];


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
    /// Builds the Board
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

        // Build the Board
        for(int i = 0; i < 3; i++)
        {
            int secondHalf = 8 - i - 1;

            for(int g = 0; g < 8; g++)
            {
                // row logic
                if(i % 2 == 0)
                {
                    // column logic
                    if(g % 2 == 0)
                    {
                        GameObject piece = GameObject.Instantiate(BlackPiecePrefab,PiecesList.transform);
                        boardGrid[g,secondHalf].setCurrentOccupant(piece.GetComponent<Piece>());
                    }
                    else
                    {
                        GameObject piece = GameObject.Instantiate(RedPiecePrefab,PiecesList.transform);
                        boardGrid[g,i].setCurrentOccupant(piece.GetComponent<Piece>());
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
                        GameObject piece = GameObject.Instantiate(RedPiecePrefab,PiecesList.transform);
                        boardGrid[g,i].setCurrentOccupant(piece.GetComponent<Piece>());
                    }
                }
            }
        }
    }


    /// <summary>
    /// Begin listening to the network
    /// 2020-11-27: WORKS
    /// </summary>
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    /// <summary>
    /// Stop listening to the network
    /// 2020-11-27: WORKS
    /// </summary>
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    /// <summary>
    /// Switches on event code
    /// 2020-11-27: WORKS
    /// </summary>
    /// <see href="https://doc.photonengine.com/en-us/pun/current/gameplay/rpcsandraiseevent#ioneventcallback_callback">IOnEventCallback Callback</see>
    public virtual void OnEvent(EventData photonEvent)
    {
        // Extract the Event Code (to switch on)
        byte eventCode = photonEvent.Code;

        
        // If someone wants to destroy a piece
        if (eventCode == pieceDestroyCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            DestroyPiece((int)data[0],(int)data[1]);
        }

        // If someone wants to move a piece
        if (eventCode == pieceMoveCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            MovePiece((int)data[0],(int)data[1],(int)data[2],(int)data[3]);
        }
    }
 
    public void RequestDestroy(int x, int y)
    {
        object[] content = new object[] { x, y};
        
        // Build the Event
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions{Receivers = ReceiverGroup.All};
        
        // Raise the Event
        PhotonNetwork.RaiseEvent(pieceDestroyCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    
    /// <summary>
    /// Destroys a Piece by LOGICAL POSITION
    /// 2020-11-27: WIP
    /// </summary>
    public virtual void DestroyPiece(int x, int y)
    {
        Piece piece = GetPieceByLoc(x,y);

        // Get the Piece and Delete it
        // gameObject != GameObject
        GameObject.Destroy(piece.gameObject);
        // Get the Space and Clear it
        GetSpaceByLoc(x,y).clearCurrentOccupant();

        
    }


    /// <summary>
    /// Uses LOGICAL POSITION
    /// MOVES the piece at START LOCATION to END LOCATION
    /// This is called from the NETWORK, not from the PLAYER
    /// It is NOT OK for this method to FAIL
    /// (Disagreement on the Network is not good)
    /// 2020-11-27: WORKS!
    /// </summary>
    public virtual void MovePiece(int startX, int startY, int endX, int endY)
    {   
        // Find the Piece
        Piece piece = GetPieceByLoc(startX,startY);
        
        // Remove Occupants from the START LOCATION
        GetSpaceByLoc(startX,startY).clearCurrentOccupant();
        
        // Move the Found Piece to the END LOCATION
        GetSpaceByLoc(endX,endY).setCurrentOccupant(piece);

        if(piece.color == Piece.PieceColor.BLACK)
        {
            if(endY == 0)
            {
                //king if not king
                if(!piece.isKing)
                {
                    piece.promotePiece();
                }
            }
        }
        else
        {
            if(endY == 7)
            {
                //king if not king
                if(!piece.isKing)
                {
                    piece.promotePiece();
                }
            }
        }
    }

    

    /// <summary>
    /// Use a LOGICAL position on board to get a <see cref="Piece" />
    /// </summary>
    public Piece GetPieceByLoc(int x, int y)
    {
        return boardGrid[x,y].getCurrentOccupant();
    }


    /// <summary>
    /// Use a LOGICAL position on the board to get a <see cref="Space.cs" />
    /// </summary>
    public Space GetSpaceByLoc(int x, int y)
    {
        return boardGrid[x,y];
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
        // Pacakge the Parameters as a New Object
        object[] content = new object[] { startX, startY, endX, endY };
        
        // Build the Event
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions{Receivers = ReceiverGroup.All};
        
        // Raise the Event
        PhotonNetwork.RaiseEvent(pieceMoveCode, content, raiseEventOptions, SendOptions.SendReliable);
    }


    private object checkSpace(int x, int y, int directionX, int directionY, Piece.PieceColor color, Piece originalPiece)
    {
        
        if(x >= 0 && x <= 7)
        {
            if(y >= 0 && y <= 7)
            {
                Space space = GetSpaceByLoc(x,y);
                if(space.getCurrentOccupant() == null)
                {
                    Debug.LogFormat("Empty Space: x-coord: {0} y-coord: {1} directionX: {2} directionY: {3}",x,y,directionX,directionY);
                    //Empty space, can jump, add to validmoves
                    ValidMove newMove = new ValidMove();
                    newMove.targetSpace = space;
                    newMove.isJump = false;
                    newMove.piece = originalPiece;
                    return newMove;
                } else
                {
                    int jumpX = x - directionX;
                    int jumpY = y + directionY;
                    if(jumpX <= 7 && jumpX >= 0)
                    {
                        if(jumpY <= 7 && jumpY >= 0)
                        {
                            if(GetPieceByLoc(x,y).color != color)
                            {
                                Space jumpSpace = GetSpaceByLoc(jumpX,jumpY);
                                if(jumpSpace.getCurrentOccupant() == null)
                                {
                                    //Add jump
                                    //Debug.LogFormat("Jump: x-coord: {0} y-coord: {1} directionX: {2} directionY: {3}",x,y,directionX,directionY);
                                    ValidMove newMove = new ValidMove();
                                    newMove.targetSpace = jumpSpace;
                                    newMove.jumped = new int[]{x,y};
                                    newMove.isJump = true;
                                    newMove.piece = originalPiece;
                                    return newMove;
                                }
                            }
                        }
                    }
                    
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Returns a List of Valid Moves
    /// Expects <see cref="PlayerManager" /> to call this method
    /// ASSUMES both <see cref="Board.RequestFirstClick" /> and <see cref="Board.RequestSecondClick" /> have run successfully
    /// </summary>
    /// <param name="startX">The Horizontal STARTING LOGICAL Location of the Move</param>
    /// <param name="startY">The vertical STARTING LOGICAL Location of the Move</param>
    /// <param name="color">Which color the PLAYER is</param>
    /// <param name="isKing">Whether the PIECE being moved is a MAN or a KING</param>
    public List<ValidMove> GetValidMoves(int startX, int startY, Piece.PieceColor color, bool isKing)
    {
        // Defaults to the direction of the red pieces
        int directionX = 1;
        int directionY = 1;
        
        // Change Direction if other player
        if(color == Piece.PieceColor.BLACK)
        {
            directionY = directionY * - 1;
        }
        
        // The Cardinal Directions to Check
        int leftX = startX - 1;
        int forwardY = startY + directionY;
        int rightX = startX + 1;

        // Where we add Valid Moves
        List<ValidMove> moves = new List<ValidMove>();
        object forwardLeftMove = checkSpace(leftX,forwardY, directionX, directionY, color, GetPieceByLoc(startX,startY));
        if(forwardLeftMove != null)
        {
            moves.Add((ValidMove)forwardLeftMove);
            //ValidMove tSpace = (ValidMove)forwardLeftMove;
            //tSpace.targetSpace.GetComponent<MeshRenderer> ().material = selectableMaterial;
        }
        object forwardRightMove = checkSpace(rightX, forwardY, -directionX, directionY, color, GetPieceByLoc(startX,startY));
        if(forwardRightMove != null)
        {
            moves.Add((ValidMove)forwardRightMove);
        }
        if(isKing)
        {
            int backY = startY - directionY;
            object backLeftMove = checkSpace(leftX,backY, directionX, -directionY,color, GetPieceByLoc(startX,startY));
            if(backLeftMove != null)
            {
                moves.Add((ValidMove)backLeftMove);
            }
            object backRightMove = checkSpace(rightX,backY,-directionX,-directionY,color, GetPieceByLoc(startX,startY));
            if(backRightMove != null)
            {
                moves.Add((ValidMove)backRightMove);
            }
        }

        bool hasJump = false;
        foreach (ValidMove move in moves)
        {
            if(move.isJump)
            {
                hasJump = true;
                break;
            }
        }
        if(hasJump)
        {
            List<ValidMove> returnMoves = new List<ValidMove>();
            foreach (ValidMove move in moves)
            {
                if(move.isJump)
                    returnMoves.Add(move);
            }
            return returnMoves;
        }
        return moves;
    }

    public List<ValidMove> GetAllValidMoves(Piece.PieceColor color)
    {
        List<ValidMove> moves = new List<ValidMove>();
        foreach (Space space in boardGrid)
        {
            if(space.getCurrentOccupant() != null && space.getCurrentOccupant().color == color)
            {
                moves.AddRange(GetValidMoves(space.x,space.y,color,space.getCurrentOccupant().isKing));
            }
        }
        List<ValidMove> returnMoves = new List<ValidMove>();
        bool hasJump = false;
        foreach(ValidMove move in moves)
        {
            if(move.isJump)
            {
                hasJump = true;
                break;
            }
        }
        if(hasJump)
        {
            foreach(ValidMove move in moves)
            {
                if (move.isJump)
                    returnMoves.Add(move);
            }
            return returnMoves;
        }
        return moves;
    }
}