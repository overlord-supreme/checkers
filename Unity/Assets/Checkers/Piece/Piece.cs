using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// <see href="https://stackoverflow.com/questions/6709072/c-getter-setter" />
/// </summary>
public class Piece : MonoBehaviour
{

    // Allow us to Promote to King
    [SerializeField] private GameObject king;


    // Identify which player we belong to
    public enum PieceColor {BLACK, RED, NONE};
    [SerializeField] public PieceColor color {get;private set;} = PieceColor.BLACK;


    /// <summary>
    /// Why is this here?
    /// </summary>
    public void setColor(PieceColor color)
    {
        this.color = color;
    }


    /// <summary>
    /// Promote a Man to a King
    /// </summary>
    public void promotePiece()
    {
        // Display the "Crown"
        king.SetActive(true);
    }
}
