using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// RESPONSIBILITIES:
///     Mouse Input
///     Turn Control
///     Move Requests
/// <see href="//https://answers.unity.com/questions/34795/how-to-perform-a-mouse-click-on-game-object.html">How To Perform A Mouse Click On Game Object</see>
/// <see href="https://www.raywenderlich.com/5441-how-to-make-a-chess-game-with-unity">How to Make a Chess Game with Unity</see>
/// </summary>
public class PlayerManager : MonoBehaviour
{

    // Where we check for mouse clicks
    [SerializeField] public LayerMask mask;


    // Which kind of player is this?
    public Piece.PieceColor color = Piece.PieceColor.BLACK;


    // Determines if we can interact with the board
    // Set by Board
    public bool currentPlayer = false;
    
    // Holds a reference to the currently selected piece of the user
    // Set below in the raycastMouse() function
    private Piece currentPieceSelected = null;
    private Space currentSpaceSelected = null;

    private List<ValidMove> moves = new List<ValidMove>();


    private void Start() {
        if(PhotonNetwork.IsMasterClient)
        {
            color = Piece.PieceColor.BLACK;
        }
        else
        {
            color = Piece.PieceColor.RED;
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
    /// FIXME: PETER: CONSOLIDATE/REMOVE <see cref="RequestFirstClick" /> and <see cref="RequestFirstClick" />
    /// 2020-11-27: Handles BOTH Piece Detection/Picking, Board Communication
    /// We Expect ONLY Tile GameObjects (Locked by mask)
    /// FIXME: DOCS INVALID
    /// </summary>
    void raycastMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
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

                // (FIRST CLICK) Find a Piece
                if(currentPieceSelected == null)
                {
                    SelectPiece(space);
                    if(currentPieceSelected != null)
                    {
                        moves = Board.getInstance().GetValidMoves(space.x, space.y, currentPieceSelected.color, currentPieceSelected.isKing);
                    }
                }
                // (SECOND CLICK) Find and Move to New Space
                else if(currentPieceSelected != null)
                {
                    // Check the Valid Moves
                    foreach (ValidMove move in moves)
                    {
                        // If the Desired Move is in Valid Moves
                        if (move.targetSpace == space)
                        {
                            // Execute the Move
                            MovePiece(space);
                            
                            // SPECIAL CASE: Is a Jump (Deletes Pieces)
                            if(move.isJump)
                            {
                                DeletePiece(move.jumped[0],move.jumped[1]);
                            }
                            
                            // Reset the variables and break
                            currentSpaceSelected = null;
                            currentPieceSelected = null;
                            break;
                        }
                    }
                    currentSpaceSelected = null;
                    currentPieceSelected = null;
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

        // Reset the variables and return on a valid click
        currentSpaceSelected = null;
        currentPieceSelected = null;
        return;
    }





    /// <summary>
    ///
    /// </summary>
    private void DeletePiece(int x, int y)
    {
        Board.getInstance().RequestDestroy(x, y);
    }




}
