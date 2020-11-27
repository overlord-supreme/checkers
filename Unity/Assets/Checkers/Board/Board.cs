﻿using System.Collections;
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
            // FIXME MR E
            int secondHalf = 8 - i - 1;
            
            // FIXME MR E
            for(int g = 0; g < 8; g++)
            {
                // FIXME MR E
                // row logic
                if(i % 2 == 0)
                {
                    // FIXME MR E
                    // column logic
                    if(g % 2 == 0)
                    {
                        // FIXME MR E
                        GameObject piece = GameObject.Instantiate(BlackPiecePrefab,PiecesList.transform);
                        boardGrid[g,secondHalf].setCurrentOccupant(piece.GetComponent<Piece>());
                    }
                    else
                    {
                        // FIXME MR E
                        GameObject piece = GameObject.Instantiate(RedPiecePrefab,PiecesList.transform);
                        boardGrid[g,i].setCurrentOccupant(piece.GetComponent<Piece>());
                    }
                }
                else
                {
                    if(g % 2 == 1)
                    {
                        // FIXME MR E
                        GameObject piece = GameObject.Instantiate(BlackPiecePrefab,PiecesList.transform);
                        boardGrid[g,secondHalf].setCurrentOccupant(piece.GetComponent<Piece>());
                    }
                    else
                    {
                        // FIXME MR E
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
    public void OnEvent(EventData photonEvent)
    {
        // Extract the Event Code (to switch on)
        byte eventCode = photonEvent.Code;

        object[] data = (object[])photonEvent.CustomData;
        // If someone wants to destroy a piece
        if (eventCode == pieceDestroyCode)
        {
            DestroyPiece((int)data[0],(int)data[1]);
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
    /// 2020-11-27: WIP
    /// </summary>
    private void PromotePiece(int[] logicalPositions)
    {
        // Find the Location
        int x = logicalPositions[0];
        int y = logicalPositions[1];
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
    private void DestroyPiece(int x, int y)
    {
        // Get the Piece and Delete it
        // gameObject != GameObject
        GameObject.Destroy(GetPieceByLoc(x,y).gameObject);
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
    private void MovePiece(int startX, int startY, int endX, int endY)
    {   
        // Find the Piece
        Piece piece = GetPieceByLoc(startX,startY);
        
        // Remove Occupants from the START LOCATION
        GetSpaceByLoc(startX,startY).clearCurrentOccupant();
        
        // Move the Found Piece to the END LOCATION
        GetSpaceByLoc(endX,endY).setCurrentOccupant(piece);
    }

    

    /// <summary>
    /// Use a LOGICAL position on board to get a <see cref="Piece" />
    /// </summary>
    private Piece GetPieceByLoc(int x, int y)
    {
        return boardGrid[x,y].getCurrentOccupant();
    }


    /// <summary>
    /// Use a LOGICAL position on the board to get a <see cref="Space.cs" />
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
        int direction = 1;
        
        // Change Direction if other player
        if(color == Piece.PieceColor.BLACK)
            direction = direction * - 1;
        
        // The Cardinal Directions to Check
        int leftX = startX - direction;
        int forwardY = startY + direction;
        int rightX = startX + direction;
        
        // If a SET of moves contains any moves that are jumps
        bool hasJump = false;

        // Where we add Valid Moves
        List<ValidMove> moves = new List<ValidMove>();
        
        // DOCS NEED FIXING
        if(leftX >= 0 && leftX <= 7)
        {
            if(forwardY >= 0 && forwardY <= 7)
            {
                Space space = GetSpaceByLoc(leftX,forwardY);
                if(space.getCurrentOccupant() != null)
                {
                    if(space.getCurrentOccupant().color != color)
                    {
                        int jumpLeftX = leftX - direction;
                        int jumpForwardY = forwardY + direction;
                        if(jumpLeftX >= 0)
                        {
                            if(jumpForwardY >= 0 && jumpForwardY <= 7)
                            {
                                Space jumpSpace = GetSpaceByLoc(jumpLeftX,jumpForwardY);
                                if(jumpSpace.getCurrentOccupant() == null)
                                {
                                    ValidMove newMove = new ValidMove();
                                    newMove.targetSpace = jumpSpace;
                                    newMove.jumped = new int[]{leftX,forwardY};
                                    newMove.isJump = true;
                                    moves.Add(newMove);
                                    hasJump = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ValidMove newMove = new ValidMove();
                    newMove.targetSpace = space;
                    newMove.isJump = false;
                    moves.Add(newMove);
                }
            }
        }
        
        // DOCS NEED FIXING
        if(rightX <= 7 && rightX >= 0)
        {
            if(forwardY >= 0 && forwardY <= 7)
            {
                Space space = GetSpaceByLoc(rightX,forwardY);
                if(space.getCurrentOccupant() != null)
                {
                     if(space.getCurrentOccupant().color != color)
                    {
                        int jumpRightX = rightX + direction;
                        int jumpForwardY = forwardY + direction;
                        if (jumpRightX <= 7)
                        {
                            if(jumpForwardY >= 0 && jumpForwardY <= 7)
                            {
                                Space jumpSpace = GetSpaceByLoc(jumpRightX,jumpForwardY);
                                if (jumpSpace.getCurrentOccupant() == null)
                                {
                                    ValidMove newMove = new ValidMove();
                                    newMove.targetSpace = jumpSpace;
                                    newMove.jumped = new int[]{rightX,forwardY};
                                    newMove.isJump = true;
                                    moves.Add(newMove);
                                    hasJump = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ValidMove newMove = new ValidMove();
                    newMove.targetSpace = space;
                    newMove.isJump = false;
                    moves.Add(newMove);
                }
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



}