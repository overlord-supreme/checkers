using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;




/// <summary>
/// RESPONSIBILITIES:
///     Move Requests
///     Mouse Input
///     In-Game UI Effects
/// </summary>
/// <see href="https://answers.unity.com/questions/34795/how-to-perform-a-mouse-click-on-game-object.html">How To Perform A Mouse Click On Game Object</see>
/// <see href="https://www.raywenderlich.com/5441-how-to-make-a-chess-game-with-unity">How to Make a Chess Game with Unity</see>
/// <see href="https://docs.unity3d.com/ScriptReference/AudioSource.PlayOneShot.html">AudioSource.PlayOneShot</see>
/// <see href="https://docs.unity3d.com/ScriptReference/HeaderAttribute.html">HeaderAttribute</see>
/// TODO:
/// - Add Pause Menu (with Options and Exit and Quit)
public class PlayerManager : MonoBehaviourPunCallbacks, IOnEventCallback
{



    // Where we check for (limited) mouse clicks
    // Hides non-tiles from raycasting
    [SerializeField] public LayerMask mask;


    // Which kind of player is this?
    public Piece.PieceColor color = Piece.PieceColor.BLACK;


    // Determines if we can interact with the board
    // (Updated over Network)
    public bool currentPlayer = false;
    private const byte playerSwapCode = 6;
    

    // Holds a reference to the currently selected piece of the user
    // Set below in the raycastMouse() function
    private Piece currentPieceSelected = null;
    private Space currentSpaceSelected = null;


    // Moves
    // NOTE: moves = selectedPieceMoves
    private List<ValidMove> moves = new List<ValidMove>();
    private List<ValidMove> allMoves = new List<ValidMove>();
    private bool canJump = false;
    
    // If we successfully complete a move
    private bool moved = false;

    // Make Permanent, is getting weirdly deleted
    Space space;


    // Audio
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


    // Hover
    private Space lastHoverSpace = null;
    private Space thisHoverSpace = null;




    /// <summary>
    /// Assigns our color based on who is hosting the match
    /// </summary>
    private void Start() {
        if(PhotonNetwork.IsMasterClient)
        {
            // Set Color and Current
            color = Piece.PieceColor.BLACK;
            currentPlayer = true;

            // Notify player to start
            
            // Play Sound
            roundAudio.PlayOneShot(roundStart, 1F);

            // Show UI
            // FIXME
        }
        else
        {
            // Set Color
            color = Piece.PieceColor.RED;
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
    /// Listen for network events
    /// </summary>
    /// <param name="photonEvent">The incoming network event</param>
    public void OnEvent(EventData photonEvent)
    {
        // Extract the Event Code (to switch on)
        byte eventCode = photonEvent.Code;
        
        // If we are swapping players
        if (eventCode == playerSwapCode)
        {
            SwapPlayer();
        }
    }




    /// <summary>
    /// Check for Mouse Clicks each frame
    /// </summary>
    void Update()
    {
        raycastMouse();
    }





    /// <summary>
    /// FIXME: PETER: CONSOLIDATE/REMOVE <see cref="PlayerManager.RequestFirstClick"></see> and <see cref="PlayerManager.RequestSecondClick" ></see>
    /// 2020-11-27: Handles BOTH Piece Detection/Picking, Board Communication
    /// We Expect ONLY Tile GameObjects (Locked by mask)
    /// FIXME: DOCS INVALID
    /// STEPS:
    /// - Gather ALL MOVES data (GenerateMoves) -- assign to variables also
    /// - Draw Raycast
    /// - Gather MOVE Data
    /// - Check Input Method
    /// TODO:
    /// - Add Hover Sound
    /// - Add Select Sound
    /// - Add Error Sound
    /// - Add DeSelect Sound
    /// - Repair Logic for Clarity
    /// - Move things/reorder into early-return pattern
    /// - Move MoveGeneration to PieceGeneration and All Generation
    /// </summary>
    /// HENRY!
    /// If you need help, please check my notes:
    /// - <see href="https://github.com/overlord-supreme/checkers/wiki/2020-11-27-Review">SE-181 Review of Dev Night</see>
    /// 
    /// DANGER WARNING CAUTION ERROR ALERT BUG FIXME HELP WARN FIX HACK
    /// 
    /// This section is in the middle of refactoring, it does not work right!
    /// Reference a Commit PRIOR to this commit to see it working (albeit messy)
    /// 
    /// DANGER WARNING CAUTION ERROR ALERT BUG FIXME HELP WARN FIX HACK
    void raycastMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!currentPlayer)
                return;

            Debug.Log("-1: Plain Call");

            // Draw the RayCast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, mask))
            {
                // DEBUG See the Line
                // Debug.DrawLine(ray.origin, hit.point);

                // Get the Target
                GameObject hitObject = hit.collider.gameObject;
                space = hitObject.GetComponent<Space>();
                Debug.Log("space: " + space);

                // DEBUG Print what we hit
                // Debug.Log(hitObject);
                // Debug.Log(hitObject.name);
                // Debug.Log(hitObject.transform.parent.name);


                //First we generate all legal moves
                //If we click on a piece (not empty) then we check if that piece is part of any valid move
                if (currentPieceSelected == null)
                {
                    allMoves = Board.getInstance().GetAllValidMoves(color);
                    Piece clickedPiece = space.getCurrentOccupant();
                    if (clickedPiece != null && clickedPiece.color == color)
                    {
                        bool isWithinValidMoves = false;
                        foreach (ValidMove move in allMoves)
                        {
                            if (move.piece == clickedPiece)
                            {
                                move.targetSpace.setHighlighted(true);
                                isWithinValidMoves = true;
                                moves.Add(move);
                            }
                        }
                        //if so select that piece
                        if (isWithinValidMoves)
                            SelectPiece(space);
                        //else do not select piece
                    }
                }
                //Second click
                else if (currentPieceSelected != null)
                {
                    Debug.Log("0: CLICK");
                    bool wasJump = false;
                    allMoves = Board.getInstance().GetAllValidMoves(color);
                    foreach (ValidMove move in allMoves)
                        move.targetSpace.setHighlighted(false);

                    //get all moves related to selected piece
                    // Check the Valid Moves
                    foreach (ValidMove move in moves)
                    {
                        Debug.Log("0.5: Checking Each Move");
                        Debug.Log("space: " + space);

                        // If the Desired Move is in Valid Moves
                        if (move.targetSpace == space)
                        {
                            // Execute the Move
                            if (move.isJump)
                            {
                                Debug.Log("BOING");
                                wasJump = true;
                            }

                            Debug.Log("1: Please Move");
                            MovePiece(space);

                            // SPECIAL CASE: Is a Jump (Deletes Pieces)
                            if (move.isJump)
                            {
                                DeletePiece(move.jumped[0], move.jumped[1]);
                                Debug.Log("1a: Please Die");
                            }
                            break;

                        }
                    }
                    if (wasJump)
                    {
                        allMoves.Clear();
                        moves.Clear();
                        Debug.LogFormat("currentPieceSelected is null? : {0}", (currentPieceSelected == null));
                        moves = Board.getInstance().GetValidMoves(space.x, space.y, color, currentPieceSelected.isKing);

                        if (moves.Count > 0 && moves[0].isJump)
                            return;
                    }
                    currentSpaceSelected = null;
                    currentPieceSelected = null;
                    moves.Clear();
                    //Change turn
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

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
    private void SelectPiece(Space space)
    {
        // If it is Occupied and a Friendly Piece
        if (space.getCurrentOccupant() && space.getCurrentOccupant().color == color)
        {
            // Log it
            Debug.Log("_____1st_____: yes");

            // Play UI Effect
            // FIXME

            // Play a Sound
            pieceAudio.PlayOneShot(pieceSelect, 1F);

            // Set the variables on a valid click
            currentPieceSelected = space.getCurrentOccupant();
            currentSpaceSelected = space;

            // Return
            return;
        }

        // If it is NOT occupied or NOT friendly
        // Log It
        Debug.Log("_____1st_____: NO (empty or unfriendly)");

        // Play UI Effect
        // FIXME

        // Play a Sound
        pieceAudio.PlayOneShot(pieceError, 1F);

        // Reset the variables on an invalid click
        currentSpaceSelected = null;
        currentPieceSelected = null;

        // Generate Moves for the UN-Selected Piece (clears the move variables)
        // GenerateSelectedPieceMoves();

        // Return
        return;
    }




    /// <summary>
    /// Upon a user mouse click* following* a successful mouse click
    /// Checks to see if the END LOCATION is valid
    /// Part of <see cref="Board.RequestMove" />
    /// Manages Side Effects
    /// Manages SELECTION Validation
    /// NOT the possible moves, see <see cref="Board.GetValidMoves" />
    /// </summary>
    /// <param name="space">Where we send our currently selected piece to</param>
    private void MovePiece(Space space)
    {
        // Log it
        Debug.Log("_____2nd_____: EMPTY, SELECT ME");
        Debug.Log("2: Got it, Moving");

        // Play UI Effect
        // FIXME

        // Play a Sound
        pieceAudio.PlayOneShot(pieceMove, 1F);

        // Execute the Move
        Board.getInstance().RequestMove(currentSpaceSelected.x, currentSpaceSelected.y, space.x, space.y);

        // Return
        return;
    }




    /// <summary>
    /// Requests the Board delete a piece
    /// (No effects here)
    /// </summary>
    private void DeletePiece(int x, int y)
    {
        Debug.Log("2a: Try Dying");
        Board.getInstance().RequestDestroy(x, y);
    }




    /// <summary>
    /// Called from <see cref="Board">the Board</see>
    /// </summary>
    /// <param name="color">The color of the destroyed piece</param>
    public void PieceWasDeleted(Piece.PieceColor pieceColor)
    {
        // THEY captured OUR piece
        if (pieceColor == color)
        {
            pieceEventAudio.PlayOneShot(pieceLost, 0.7F);
        }

        // WE captured THEIR piece
        if (pieceColor != color)
        {
            pieceEventAudio.PlayOneShot(pieceCapture, 1F);
        }
    }




    /// <summary>
    /// On Network Update, switches players and recomputes legal moves
    /// </summary>
    /// <see cref="PlayerManager.OnEvent">For who calls this method</see>
    private void SwapPlayer()
    {
        // Swap Players
        currentPlayer = !currentPlayer;

        // Alert next player and update their values
        if (currentPlayer)
        {
            // Play Sound
            roundAudio.PlayOneShot(roundStart, 1F);

            // Show UI
            // FIXME

            // Update Legal Moves
            // GenerateAllMoves();
        }
    }


}
