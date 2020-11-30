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
    private List<ValidMove> selectedPieceMoves = new List<ValidMove>();
    private List<ValidMove> allMoves = new List<ValidMove>();
    private bool canJump = false;
    
    // If we successfully complete a move
    private bool moved = false;


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
    /// Generates all valid NON-Hop and HOP Moves FOR ALL PIECES
    /// DOES set <see cref="PlayerManager.canJump"></see>
    /// </summary>
    /// TODO:
    /// - Move Implementation from <see cref="PlayerManager.rayCastMouse"></see> to here
    private void GenerateAllMoves()
    {

    }


    /// <summary>
    /// Generates all valid NON-Hop and HOP Moves for ONE piece
    /// Does NOT set <see cref="PlayerManager.canJump"></see>
    /// </summary>
    /// TODO:
    /// - Move Implementation from <see cref="PlayerManager.rayCastMouse"></see> to here
    private void GenerateSelectedPieceMoves()
    {

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
        // If Player is Trying to Select
        if (Input.GetMouseButtonDown(0))
        {
            // If player is NOT selecting on their turn
            if (!currentPlayer)
            {
                // Play Error Sound
                pieceAudio.PlayOneShot(pieceError, 1F);

                // Show some UI Effect
                // FIXME

                // Return
                return;
            }
        }


        // Draw the RayCast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If we find something
        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            // Generate "Current" Moves for THIS SELECTED PIECE
            // And compare against all other available moves

            // See the Line (Debugging)
            // Debug.DrawLine(ray.origin, hit.point);

            // Get the Target
            GameObject hitObject = hit.collider.gameObject;
            Space space = hitObject.GetComponent<Space>();

            // If we are only hovering
            if (!Input.GetMouseButtonDown(0))
            {
                if (lastHoverSpace != thisHoverSpace)
                {
                    // Play Sound
                    pieceAudio.PlayOneShot(pieceHover, 1F);

                    // Play UI
                    // FIXME

                    // Save It
                    lastHoverSpace = thisHoverSpace;
                }

                // Stop
                return;
            }

            // TOTAL HACK!
            // OTHERWISE, we must be SELECTING

            // First we generate all legal moves for this piece

            // TODO: Move this OUTSIDE of this method
            // begin:   first-click
            // If we click on a piece (not empty) then we check if that piece is part of any valid move
            if (!currentPieceSelected)
            {
                // Get all possible moves for the player
                allMoves = Board.getInstance().GetAllValidMoves(color);
                Piece clickedPiece = space.getCurrentOccupant();
                
                // Check that the piece is not empy and is friendly
                if(clickedPiece != null && clickedPiece.color == color)
                {
                    // Check that the piece has any moves
                    // And add all moves for this piece
                    bool isWithinValidMoves = false;
                    foreach(ValidMove move in allMoves)
                    {
                        if(move.piece == clickedPiece)
                        {
                            isWithinValidMoves = true;
                            selectedPieceMoves.Add(move);
                        }
                    }

                    // if so select that piece
                    if(isWithinValidMoves)
                    {
                        SelectPiece(space);
                    }
                    
                    // else do not select piece
                    // Make an ERROR effect (UI + Sound)
                    // FIXME
                }
                return;
            }
            // end:     first-click



            // begin:   second-click
            if (currentPieceSelected != null)
            {
                bool wasJump = false;
                
                // Check the Valid Moves for this piece
                foreach (ValidMove move in selectedPieceMoves)
                {
                    // If the Desired Move is in Valid Moves
                    if (move.targetSpace == space)
                    {
                        // If the move involved a jump (kills a piece)
                        if(move.isJump)
                        {
                            // note it and kill the victim
                            wasJump = true;
                            DeletePiece(move.jumped[0], move.jumped[1]);
                        }

                        // Execute the Move and note it
                        MovePiece(space);
                        moved = true;

                        // Stop looping
                        break;
                    }
                }

                // For multi-step hopping
                if(wasJump)
                {
                    // Reset our total moves and current selected moves
                    allMoves.Clear();
                    selectedPieceMoves.Clear();
                    
                    // Debug
                    Debug.LogFormat("currentPieceSelected is null? : {0}",(currentPieceSelected == null));
                    
                    // Get new moves
                    selectedPieceMoves = Board.getInstance().GetValidMoves(space.x, space.y, color, currentPieceSelected.isKing);

                    // If we have more than 1 new move and they are jumps, STOP, do not proceed
                    // (Implicitly lets the player GO AGAIN)
                    if(selectedPieceMoves.Count > 0 && selectedPieceMoves[0].isJump)
                    {
                        // Possible Bug? (FIXME)
                        return;
                    }
                }

                // Only change turns IF we moved, otherwise do not give up
                if (moved)
                {
                    // Clear References
                    currentSpaceSelected = null;
                    currentPieceSelected = null;
                    selectedPieceMoves.Clear();

                    // Change turn
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                    PhotonNetwork.RaiseEvent(playerSwapCode, true, raiseEventOptions, SendOptions.SendReliable);

                    // Reset Move
                    moved = false;
                }

                // Return
                return;
            }
            // end:     second-click
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
        {
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
            GenerateAllMoves();
        }
    }


}
