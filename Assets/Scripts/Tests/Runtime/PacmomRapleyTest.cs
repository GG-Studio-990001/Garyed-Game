using NUnit.Framework;
using Runtime.CH1.Pacmom;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    [TestFixture]
    public class PacmomRapleyTest
    {
        private GameObject player;
        private Rapley rapley;
        private RapleyMovement rapleyMovement;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            player = new GameObject("Player");
            rapley = player.AddComponent<Rapley>();
            rapleyMovement = player.AddComponent<RapleyMovement>();
            rapley.movement = rapleyMovement;

            yield return new WaitForFixedUpdate();
        }

        [UnityTest]
        public IEnumerator TestRapleyMovement()
        {
            rapleyMovement.rigid = player.GetComponent<Rigidbody2D>();

            rapleyMovement.rigid.position = Vector3.zero;
            rapleyMovement.SetDirection(new Vector2(1, 0));
            rapleyMovement.Move();

            yield return new WaitForFixedUpdate();

            Assert.AreNotEqual(Vector3.zero, player.GetComponent<RapleyMovement>().rigid.position);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(player);

            yield return new WaitForFixedUpdate();
        }
    }
}
