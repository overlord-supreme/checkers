using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RESPONSIBILITIES:
///     Mouse Input
///     Turn Control
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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    /// <summary>
    /// Check for Mouse Clicks
    /// </summary>
    void Update()
    {
        raycastMouse();
    }


    /// <summary>
    /// Check for Mouse Clicks
    /// Does a thing if it hits something
    /// 2020-11-27T11:04: INCOMPLETE, DOES find target, DOES NOT talk to <see cref="Board.cs" />
    /// </summary>
    void raycastMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if  (Physics.Raycast(ray,  out hit, 100,  mask))
            {
                Debug.Log("Hit");
                Debug.DrawLine(ray.origin, hit.point);
                GameObject hitObject = hit.collider.gameObject;
                Space space = hitObject.GetComponent<Space>();
                // Print what we hit
                Debug.Log(hitObject);
                Debug.Log(hitObject.name);
                Debug.Log(hitObject.transform.parent.name);
                //If we do not already have a piece selected
                if(currentPieceSelected == null)
                {
                    //if they are our piece, select them
                    if(space.getCurrentOccupant() != null && space.getCurrentOccupant().color == color)
                    {
                        Board.getInstance().RequestFirstClick(space, color);
                        currentPieceSelected = space.getCurrentOccupant();
                        currentSpaceSelected = space;
                    }
                }
                else
                {
                    if(space.getCurrentOccupant() == null)
                    {
                        Board.getInstance().RequestSecondClick(space);
                        Board.getInstance().RequestMove(currentSpaceSelected.x,currentSpaceSelected.y,space.x,space.y);
                        currentSpaceSelected = null;
                        currentPieceSelected = null;
                    }
                }
            }
        }
    }


}
