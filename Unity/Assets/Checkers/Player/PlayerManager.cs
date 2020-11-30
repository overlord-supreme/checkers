using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Space = BoardSpace.Space;

/// <summary>
/// RESPONSIBILITIES:
///     Mouse Input
///     Turn Control
///     Move Requests
/// <see href="https://answers.unity.com/questions/34795/how-to-perform-a-mouse-click-on-game-object.html">How To Perform A Mouse Click On Game Object</see>
/// <see href="https://www.raywenderlich.com/5441-how-to-make-a-chess-game-with-unity">How to Make a Chess Game with Unity</see>
/// <see href="https://docs.unity3d.com/ScriptReference/AudioSource.PlayOneShot.html">AudioSource.PlayOneShot</see>
/// <see href="https://docs.unity3d.com/ScriptReference/HeaderAttribute.html">HeaderAttribute</see>
/// </summary>
public class PlayerManager : MonoBehaviourPunCallbacks, IOnEventCallback
{

    // Where we check for (limited) mouse clicks
    [SerializeField] public LayerMask mask;


    // Which kind of player is this?
    public Piece.PieceColor color = Piece.PieceColor.BLACK;


    // Determines if we can interact with the board
    // Set by Board
    public bool currentPlayer = false;
    
    private const byte playerSwapCode = 6;
    
    // Holds a reference to the currently selected piece of the user
    // Set below in the raycastMouse() function
    public Piece currentPieceSelected = null;
    public Space currentSpaceSelected = null;

    private List<ValidMove> moves = new List<ValidMove>();
    private List<ValidMove> allMoves = new List<ValidMove>();

    [Header("Piece Audio")]
    public AudioSource pieceAudio;
    public AudioClip pieceHover;
    public AudioClip pieceSelect;
    public AudioClip pieceDeselect;
    public AudioClip pieceMove;
    public AudioClip pieceError;
    
    [Header("Piece Event Audio")]
    public AudioSource pieceEventAudio;
    public AudioClip pieceCapture;
    public AudioClip pieceLost;
    
    [Header("Round Audio")]
    public AudioSource roundAudio;
    public AudioClip roundStart;
    public AudioClip gameOver;

    public virtual void Start() {
        if(PhotonNetwork.IsMasterClient)
        {
            color = Piece.PieceColor.BLACK;
            currentPlayer = true;
        }
        else
        {
            color = Piece.PieceColor.RED;
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

    public void OnEvent(EventData photonEvent)
    {
        // Extract the Event Code (to switch on)
        byte eventCode = photonEvent.Code;
        
        if (eventCode == playerSwapCode)
        {
            SwapPlayer();
        }
    }




    /// <summary>
    /// Check for Mouse Clicks each frame
    /// </summary>
    public void Update()
    {
        raycastMouse();
    }




    /// <summary>
    /// FIXME: PETER: CONSOLIDATE/REMOVE <see cref="RequestFirstClick" /> and <see cref="RequestFirstClick" />
    /// 2020-11-27: Handles BOTH Piece Detection/Picking, Board Communication
    /// We Expect ONLY Tile GameObjects (Locked by mask)
    /// FIXME: DOCS INVALID
    /// </summary>
    public virtual void raycastMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!currentPlayer)
                return;
            
            // Draw the RayCast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, mask))
            {
                // DEBUG See the Line
                // Debug.DrawLine(ray.origin, hit.point);
                
                // Get the Target
                GameObject hitObject = hit.collider.gameObject;
                Space space = hitObject.GetComponent<Space>();
                
                // DEBUG Print what we hit
                // Debug.Log(hitObject);
                // Debug.Log(hitObject.name);
                // Debug.Log(hitObject.transform.parent.name);


                //First we generate all legal moves
                //If we click on a piece (not empty) then we check if that piece is part of any valid move
                if(currentPieceSelected == null)
                {
                    allMoves = Board.getInstance().GetAllValidMoves(color);
                    Piece clickedPiece = space.getCurrentOccupant();
                    if(clickedPiece != null && clickedPiece.color == color)
                    {
                        bool isWithinValidMoves = false;
                        foreach(ValidMove move in allMoves)
                        {
                            if(move.piece == clickedPiece)
                            {
                                move.targetSpace.setHighlighted(true);
                                isWithinValidMoves = true;
                                moves.Add(move);
                            }
                        }
                        //if so select that piece
                        if(isWithinValidMoves)
                            SelectPiece(space);
                        //else do not select piece
                    }
                }
                //Second click
                else if(currentPieceSelected != null)
                {
                    bool wasJump = false;
                    allMoves = Board.getInstance().GetAllValidMoves(color);

                    bool wasValid = false;
                    //get all moves related to selected piece
                    // Check the Valid Moves
                    foreach (ValidMove move in moves)
                    {
                        // If the Desired Move is in Valid Moves
                        if (move.targetSpace == space)
                        {
                            // Execute the Move
                            if(move.isJump)
                                wasJump = true;
                            MovePiece(space);
                            
                            // SPECIAL CASE: Is a Jump (Deletes Pieces)
                            if(move.isJump)
                            {
                                DeletePiece(move.jumped[0],move.jumped[1]);
                            }
                            wasValid = true;
                            break;
                            
                        }
                    }
                    if(!wasValid)
                    {
                        return;
                    }
                    foreach (ValidMove move in allMoves)
                        move.targetSpace.setHighlighted(false);
                    if(wasJump)
                    {
                        allMoves.Clear();
                        moves.Clear();
                        Debug.LogFormat("currentPieceSelected is null? : {0}",(currentPieceSelected == null));
                        moves = Board.getInstance().GetValidMoves(space.x,space.y,color,currentPieceSelected.isKing);

                        if(moves.Count > 0 && moves[0].isJump)
                        {
                            foreach (ValidMove move in moves)
                            {
                                move.targetSpace.setHighlighted(true);
                            }
                            return;
                        }
                    }
                    currentSpaceSelected = null;
                    currentPieceSelected = null;
                    moves.Clear();
                    //Change turn
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions{Receivers = ReceiverGroup.All};

                    // Raise the Event
                    PhotonNetwork.RaiseEvent(playerSwapCode, true, raiseEventOptions, SendOptions.SendReliable);

                }
            }
        }
    }




    /// <summary>
    /// Upon a user mouse click
    /// Checks to see if the START LOCATION is valid
    /// Part of <see cref="Board.RequestMove" />
    /// Empty/Enemy Space = Highlight with "Not Selected"
    /// Friendly Space = Highlight with "Selected"
    /// Manages SELECTION Validation, NOT move Validation
    /// NOT the possible moves, see <see cref="Board.GetValidMoves" />
    /// Does not Return anything, instead sets <see cref="currentPieceSelected" /> and <see cref="currentSpaceSelected" />
    /// </summary>
    public void SelectPiece(Space space)
    {
        // If it is Occupied and a Friendly Piece
        if (space.getCurrentOccupant() != null && space.getCurrentOccupant().color == color)
        {
            // Show some effect
            // REPLACE ME WITH NICE SIDE EFFECTS!!! (TODO:CHRIS)
            Debug.Log("FIRST click: FRIENDLY, SELECT ME");
            
            // Reset the variables and return on a valid click
            currentPieceSelected = space.getCurrentOccupant();
            currentSpaceSelected = space;
            return;
        }
        
        // If it is NOT occupied or NOT friendly
        // Show some effect
        // REPLACE ME WITH NICE SIDE EFFECTS!!! (TODO:CHRIS)
        Debug.Log("FIRST click: FAILURE       EMPTY/UNFRIENDLY, DO NOT select me");
        
        // Reset the variables and return on an invalid click
        currentSpaceSelected = null;
        currentPieceSelected = null;
        return;
    }




    /// <summary>
    /// Upon a user mouse click *following* a successful mouse click
    /// Checks to see if the END LOCATION is valid
    /// Part of <see cref="Board.RequestMove" />
    /// Manages Side Effects
    /// Manages SELECTION Validation
    /// NOT the possible moves, see <see cref="Board.GetValidMoves" />
    /// </summary>
    private void MovePiece(Space space)
    {
        Debug.Log("SECOND click: EMPTY, SELECT ME");
        
        // Execute the Move
        Board.getInstance().RequestMove(currentSpaceSelected.x, currentSpaceSelected.y, space.x, space.y);
        currentSpaceSelected = space;
        return;
    }




    /// <summary>
    ///
    /// </summary>
    private void DeletePiece(int x, int y)
    {
        Board.getInstance().RequestDestroy(x, y);
    }




    public virtual void SwapPlayer()
    {
        currentPlayer = !currentPlayer;
    }


}
