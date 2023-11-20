using NUnit.Framework;
using Runtime.CH1.Pacmom;
using UnityEngine;

namespace Tests.Editor
{
    [TestFixture]
    public class PacmomRapleyTest
    {
        private GameObject manager;
        private GameObject player;

        [SetUp]
        public void SetUp()
        {
            player = new GameObject("Player");
            player.AddComponent<Rapley>();
            player.AddComponent<RapleyMovement>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(player);
        }

        [Test]
        public void TestRapleyMove()
        {
            player.GetComponent<RapleyMovement>().rigid = player.GetComponent<Rigidbody2D>();

            player.GetComponent<RapleyMovement>().rigid.position = Vector3.zero;
            player.GetComponent<RapleyMovement>().SetDirection(new Vector2(1, 0));
            player.GetComponent<RapleyMovement>().Move();

            Assert.AreNotEqual(Vector3.zero, player.GetComponent<RapleyMovement>().rigid.position);
        }

    }
}
