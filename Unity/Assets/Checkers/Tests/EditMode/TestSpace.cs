using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Space = BoardSpace.Space;

namespace Tests
{
    public class TestSpace
    {
        [Test]
        public void TestAssignAndClearSpace()
        {
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
            Assert.AreEqual(space.getCurrentOccupant(), piece);
            Assert.IsTrue(space.isOccupied());

            space.clearCurrentOccupant();
            Assert.IsNull(space.getCurrentOccupant());
        }
    }
}
