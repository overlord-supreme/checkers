using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using ExitGames.Client.Photon;
using Space = BoardSpace.Space;

namespace Tests
{
    public class TestBoard
    {
        [Test]
        public void TestGetPieceByLocation()
        {
            Board board = new Board();
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<Space>();
            Space space = gameObject.GetComponent<Space>();
            Assert.IsFalse(space.isOccupied());

            GameObject pieceObj = new GameObject();
            pieceObj.AddComponent<Piece>();
            var piece = pieceObj.GetComponent<Piece>();
            space.piecePosition = space.transform;
            space.transform.position = new Vector3(2, 2, 2);
            space.setCurrentOccupant(piece);

            board.boardGrid[0, 0] = space;
            Assert.AreEqual(board.GetSpaceByLoc(0, 0), space);
            Assert.AreEqual(board.GetPieceByLoc(0, 0), piece);

            space.clearCurrentOccupant();
            Assert.IsNull(board.GetPieceByLoc(0, 0));
            Assert.IsNotNull(board.GetSpaceByLoc(0, 0));
        }
    }
}
