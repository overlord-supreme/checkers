using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;




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
/// <see href="https://doc.photonengine.com/en-us/pun/current/gameplay/rpcsandraiseevent">RPCs and RaiseEvent | Photon Engine</see>
public class Board : MonoBehaviourPunCallbacks, IOnEventCallback
{

    // Piece Management (LOCAL)
    [SerializeField] private GameObject RedPiecePrefab;
    [SerializeField] private GameObject BlackPiecePrefab;
    [SerializeField] private GameObject PiecesList;
    public Material selectableMaterial;


    // How we Track Logical Position
    private Space[,] boardGrid = new Space[8,8];


    // Totally a hack, no "gameObject" in "MonoBehaviourPunCallbacks"
    // Publicly set from the Inspector
    public GameObject gameObject;


    // Player Management
    public enum PlayerColor {BLACK, RED, NONE};


    // Event Codes
    private const byte pieceMoveCode =      1;
    private const byte pieceDestroyCode =   3;


    // Board MUST BE SINGLETON
    private static Board singleton = null;
    public static Board getInstance() {return singleton;}


    // Audio
    public AudioSource boardAudio;
    public AudioClip gameOver;


    // Local Player
    public PlayerManager player;


    /// <summary>
    /// Sets up Board with Single Reference and Logical Positions
    /// Builds the Board
    /// </summary>
    private void Start() 
    {
        // Ensure this is the only Board in Existence
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
        
        // Load the Tile LOGICAL POSITIIONS into our 2D LOOKUP array
        Space[] spaces = gameObject.GetComponentsInChildren<Space>();
        foreach(Space space in spaces)
        {
            boardGrid[space.x, space.y] = space;
        }

        // Build the Board
        BuildBoard();

        // Find the Local Player
        try
        {
            player = GameObject.Find("Player").GetComponent<PlayerManager>();
        }
        catch
        {
            Debug.Log("Could not find the local player");
        }
    }




    /// <summary>
    /// Builds the Board
    /// Called from <see cref="Board.Start"></see>
    /// </summary>
    private void BuildBoard()
    {
        // (rows of 3, both on top and on bottom, built at the same time)
        // top row is RED
        for (int topRow = 0; topRow < 3; topRow++)
        {
            // bottom row is BLACK
            int bottomRow = 8 - topRow - 1;

            // for each column
            for (int column = 0; column < 8; column++)
            {
                // if ROW is even
                if (topRow % 2 == 0)
                {
                    // if column is even
                    if (column % 2 == 0)
                    {
                        // Add a Black Piece to the Bottom
                        GameObject piece = GameObject.Instantiate(BlackPiecePrefab, PiecesList.transform);
                        boardGrid[column, bottomRow].setCurrentOccupant(piece.GetComponent<Piece>());
                    }
                    // if column is odd
                    else
                    {
                        // Add a Red Piece to the Top
                        GameObject piece = GameObject.Instantiate(RedPiecePrefab, PiecesList.transform);
                        boardGrid[column, topRow].setCurrentOccupant(piece.GetComponent<Piece>());
                    }
                }
                // if ROW is odd
                else
                {
                    // if column is odd
                    if (column % 2 == 1)
                    {
                        // Add a Black Piece to the Bottom
                        GameObject piece = GameObject.Instantiate(BlackPiecePrefab, PiecesList.transform);
                        boardGrid[column, bottomRow].setCurrentOccupant(piece.GetComponent<Piece>());
                    }
                    // if column is even
                    else
                    {
                        // Add a Redd Piece to the Top
                        GameObject piece = GameObject.Instantiate(RedPiecePrefab, PiecesList.transform);
                        boardGrid[column, topRow].setCurrentOccupant(piece.GetComponent<Piece>());
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
    public void OnEvent(EventData photonEvent)
    {
        // Extract the Event Code (to switch on)
        byte eventCode = photonEvent.Code;
        
        // If someone wants to destroy a piece
        if (eventCode == pieceDestroyCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            DestroyPiece((int)data[0], (int)data[1]);
        }

        // If someone wants to move a piece
        if (eventCode == pieceMoveCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            MovePiece((int)data[0], (int)data[1], (int)data[2], (int)data[3]);
        }
    }




    /// <summary>
    /// Allow Players to Request the Destrction of a Piece on the Board
    /// Builds and Issues the Network Request
    /// Sends to Everyone
    /// </summary>
    /// <param name="x">The Horizontal (Column) Position of the Piece</param>
    /// <param name="y">The Vertical (Row) Position of the Piece</param>
    public void RequestDestroy(int x, int y)
    {
        // Build the Packet Data
        object[] content = new object[] {x, y};
        
        // Build the Event
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions{Receivers = ReceiverGroup.All};
        
        // Raise the Event
        PhotonNetwork.RaiseEvent(pieceDestroyCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    



    /// <summary>
    /// Destroys a Piece by LOGICAL POSITION
    /// </summary>
    private void DestroyPiece(int x, int y)
    {
        // Find the Piece
        Piece piece = GetPieceByLoc(x,y);

        // Destroy it
        GameObject.Destroy(piece.gameObject);

        // Get the Space and Clear it
        GetSpaceByLoc(x,y).clearCurrentOccupant();

        // Let the Local player know
        player.PieceWasDeleted(piece.color);
    }




    /// <summary>
    /// Uses LOGICAL POSITION
    /// MOVES the piece at START LOCATION to END LOCATION
    /// This is called from the NETWORK, not from the PLAYER
    /// It is NOT OK for this method to FAIL
    /// (Disagreement on the Network is not good)
    /// Assumes end space is EMPTY
    /// </summary>
    /// <param name="startX">Which Column we are moving FROM</param>
    /// <param name="startY">Which Row we are moving FROM</param>
    /// <param name="endX">Which Column we are moving TO</param>
    /// <param name="endY">Which Row we are moving TO</param>
    private void MovePiece(int startX, int startY, int endX, int endY)
    {   
        // Find the Piece
        Piece piece = GetPieceByLoc(startX, startY);
        
        // Remove Occupants from the START LOCATION
        GetSpaceByLoc(startX, startY).clearCurrentOccupant();
        
        // Move the Found Piece to the END LOCATION
        GetSpaceByLoc(endX, endY).setCurrentOccupant(piece);

        // Check for Kinging
        CheckAndPromote(piece, endY);
    }




    /// <summary>
    /// Check if the Piece should be promoted and do so
    /// </summary>
    /// <param name="piece">The piece we are inspecting</param>
    /// <param name="endY">The Row of the Piece</param>
    private void CheckAndPromote(Piece piece, int endY)
    {
        // Do not promote kings to kings
        if (piece.isKing)
        {
            return;
        }

        // BLACK
        if (piece.color == Piece.PieceColor.BLACK)
        {
            if (endY == 0)
            {
                piece.promotePiece();
            }
            return;
        }

        // RED
        if (piece.color == Piece.PieceColor.RED)
        {
            if (endY == 7)
            {
                piece.promotePiece();
            }
            return;
        }
    }



    /// <summary>
    /// Use a LOGICAL position on board to get a <see cref="Piece"></see>
    /// </summary>
    /// <param name="x">Column of Piece</param>
    /// <param name="y">Row of Piece</param>
    private Piece GetPieceByLoc(int x, int y)
    {
        return boardGrid[x,y].getCurrentOccupant();
    }




    /// <summary>
    /// Use a LOGICAL position on the board to get a <see cref="Space"></see>
    /// </summary>
    /// <param name="x">Column of Piece</param>
    /// <param name="y">Row of Piece</param>
    /// <returns></returns>
    private Space GetSpaceByLoc(int x, int y)
    {
        return boardGrid[x, y];
    }




    /// <summary>
    /// This takes in a Player's request
    /// Does NOT check to see if it is legal
    /// DOES makes a MovePiece REQUEST on the network
    /// </summary>
    /// <param name="startX">Which Column we are moving FROM</param>
    /// <param name="startY">Which Row we are moving FROM</param>
    /// <param name="endX">Which Column we are moving TO</param>
    /// <param name="endY">Which Row we are moving TO</param>
    public void RequestMove(int startX, int startY, int endX, int endY)
    {
        // Pacakge the Parameters as a New Object
        object[] content = new object[] { startX, startY, endX, endY };
        
        // Build the Event
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions{Receivers = ReceiverGroup.All};
        
        // Raise the Event
        PhotonNetwork.RaiseEvent(pieceMoveCode, content, raiseEventOptions, SendOptions.SendReliable);
    }




    /// <summary>
    /// FIXME
    /// </summary>
    /// <param name="currentX">The Current Position of the PIECE (Column)</param>
    /// <param name="y">The Current Position of the PIECE (Row)</param>
    /// <param name="directionX">The X Direction that the PIECE CAN move in</param>
    /// <param name="directionY">The Y Direction that the PIECE CAN move in (RED v BLACK)</param>
    /// <param name="color">The PLAYER's color</param>
    /// <param name="originalPiece">UNKNOWN (FIXME)</param>
    /// <returns>An Object of all the Possible Move</returns>
    /// TODO: CLEAN THIS UP:
    /// - Use Early Returns (Limit Indent SIGNIFICANTLY)
    /// - Finish Renaming variables
    private object checkSpace(int currentX, int y, int directionX, int directionY, Piece.PieceColor color, Piece originalPiece)
    {
        // If the piece is not Horizontally out of bounds
        if (currentX >= 0 && currentX <= 7)
        {
            // If the piece is not Vertically out of bounds
            if (y >= 0 && y <= 7)
            {
                // Find the Space
                Space space = GetSpaceByLoc(currentX, y);

                // If the piece is empty
                if (space.getCurrentOccupant() == null)
                {
                    // Just add it as a move and stop
                    Debug.LogFormat("Empty Space: x-coord: {0} y-coord: {1} directionX: {2} directionY: {3}", currentX, y, directionX, directionY);
                    //Empty space, can jump, add to validmoves
                    ValidMove newMove = new ValidMove();
                    newMove.targetSpace = space;
                    newMove.isJump = false;
                    newMove.piece = originalPiece;
                    return newMove;
                }
                else
                {
                    // calculate the jump
                    int jumpX = currentX - directionX;
                    int jumpY = y + directionY;

                    // if the Jump is not Horizontally out of bounds
                    if (jumpX <= 7 && jumpX >= 0)
                    {
                        // if the Jump is not Vertically out of bounds
                        if (jumpY <= 7 && jumpY >= 0)
                        {
                            // if the piece is not the same color as the player
                            if (GetPieceByLoc(currentX, y).color != color)
                            {
                                // Find the Space
                                Space jumpSpace = GetSpaceByLoc(jumpX,jumpY);

                                // if the Jump EndSpace is empty
                                if (jumpSpace.getCurrentOccupant() == null)
                                {
                                    // add it as a move and stop
                                    //Debug.LogFormat("Jump: x-coord: {0} y-coord: {1} directionX: {2} directionY: {3}", currentX, y, directionX, directionY);
                                    ValidMove newMove = new ValidMove();
                                    newMove.targetSpace = jumpSpace;
                                    newMove.jumped = new int[]{ currentX, y};
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

        // Anything Else, Cannot Proceed, Return No Moves
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
    /// TODO:
    /// - add comments
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




    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    /// TODO:
    /// - add param description
    /// - add returns description
    /// - add comments/general cleanup
    /// - add summary
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