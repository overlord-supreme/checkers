using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;




/// <summary>
/// 
/// </summary>
/// <seealso cref="https://doc.photonengine.com/en-us/pun/v1/demos-and-tutorials/package-demos/rockpaperscissors-demo">RPCs and RaiseEvent | Photon Engine</seealso>
public class Board : MonoBehaviourPunCallbacks, IOnEventCallback
{

    // Piece Management (LOCAL)
    [SerializeField] private GameObject WhitePiecePrefab;
    [SerializeField] private GameObject BlackPiecePrefab;
    [SerializeField] private GameObject PiecesList;
    private Space[,] boardGrid = new Space[8,8];

    // Event Codes
    private const byte pieceSpawnCode = 1;
    private const byte pieceMoveCode = 2;

    [SerializeField] private GameObject TempLoc;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        // If someone wants to spawn a piece
        if (eventCode == pieceSpawnCode)
        {
            SpawnPiece();
        }
    }

    private void SpawnPiece()
    {
        GameObject temp = Instantiate(WhitePiecePrefab, PiecesList.transform);
        temp.transform.position = TempLoc.transform.position;
    }

    void Update()
    {
        // If the user wants to spawn a piece
        // TEMP!!!! (@020-11-25)
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Send this event to EVERYONE
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            
            // We are Spawning a Piece, with the MessageConent of "True", Reliably
            PhotonNetwork.RaiseEvent(pieceSpawnCode, true, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
