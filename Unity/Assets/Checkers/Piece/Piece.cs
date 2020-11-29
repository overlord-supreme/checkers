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

    public bool kinged { get; private set; } = false;

    // Identify which player we belong to
    public enum PieceColor {BLACK, RED, NONE};

    // The current color of the piece.
    [SerializeField] public PieceColor color {get; set;} = PieceColor.BLACK;

    /// <summary>
    /// Promote a Man to a King
    /// </summary>
    public void promotePiece()
    {
        // Display the "Crown"
        king.SetActive(true);
        kinged = true;
    }
}
