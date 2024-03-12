using NUnit.Framework;
using Runtime.CH1.Pacmom;
using Runtime.ETC;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime.PacmomGameTest
{
    [TestFixture]
    public class DustTest
    {
        private GameObject dustObj;
        private Dust dust;
        private MovementAndEyes dustMovement;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            dustObj = new GameObject("DustObj");
            dust = dustObj.AddComponent<Dust>();
            dustMovement = dustObj.AddComponent<MovementAndEyes>();
            dust.movement = dustMovement;

            yield return new WaitForFixedUpdate();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(dustObj);

            yield return new WaitForFixedUpdate();
        }

        [UnityTest]
        public IEnumerator DustMovement()
        {
            dustMovement.SetRigidBody(dustObj.GetComponent<Rigidbody2D>());

            dustMovement.rigid.position = Vector3.zero;
            dustMovement.SetNextDirection(new Vector2(1, 0));

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(dustMovement.rigid.position.x > 0);
        }
    }
}