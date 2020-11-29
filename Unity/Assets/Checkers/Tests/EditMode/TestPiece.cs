using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestPiece
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestPromotingPiece() 
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<Piece>();
            Piece piece = gameObject.GetComponent<Piece>();
            piece.king = new GameObject();
            Assert.IsFalse(piece.isKing);
            piece.promotePiece();
            Assert.IsTrue(piece.isKing);
        }
    }
}
