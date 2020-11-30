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
    public class TestPlayer
    {
        [Test]
        public void TestSwapPlayerOnPlayerSwapCode() 
        {
            // Send event for player swap (6)
            var mockPlayerManager = new Mock<PlayerManager>() { CallBase = true };
            EventData playerSwapCode = new EventData();
            EventData notPlayerSwapCode = new EventData();
            notPlayerSwapCode.Code = 2;
            playerSwapCode.Code = 6;
            mockPlayerManager.Object.OnEvent(playerSwapCode);
            mockPlayerManager.Object.OnEvent(notPlayerSwapCode);
            mockPlayerManager.Object.OnEvent(playerSwapCode);
            mockPlayerManager.Verify(x => x.SwapPlayer(), Times.Exactly(2));
        }

        [Test]
        public void TestRaycastOnUpdate() 
        {
            var mockPlayerManager = new Mock<PlayerManager>() { CallBase = true };
            mockPlayerManager.Setup(x => x.raycastMouse()).Verifiable();
            mockPlayerManager.Object.Update();
            mockPlayerManager.Object.Update();
            mockPlayerManager.Verify(x => x.raycastMouse(), Times.Exactly(2));
        }

        [Test]
        public void TestSelectPiece()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<Space>();
            Space space = obj.GetComponent<Space>();

            GameObject obj2 = new GameObject();
            obj2.AddComponent<Piece>();
            Piece piece = obj2.GetComponent<Piece>();
            piece.color = 0;
            space.piecePosition = space.transform;
            space.transform.position = new Vector3(2, 2, 2);
            space.setCurrentOccupant(piece);

            GameObject obj3 = new GameObject();
            obj3.AddComponent<PlayerManager>();
            PlayerManager pm = obj3.GetComponent<PlayerManager>();
            pm.SelectPiece(space);

            Assert.AreEqual(pm.currentPieceSelected, piece);
            Assert.AreEqual(pm.currentSpaceSelected, space);
        }
    }
}
