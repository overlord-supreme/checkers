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
    public enum PlayerColor {BLACK, WHITE, NONE};


    // Determines if we can interact with the board
    // Set by Board
    public bool currentPlayer = false;
    
    
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

                // Print what we hit
                Debug.Log(hitObject);
                Debug.Log(hitObject.name);
                Debug.Log(hitObject.transform.parent.name);
            }
        }
    }


}
