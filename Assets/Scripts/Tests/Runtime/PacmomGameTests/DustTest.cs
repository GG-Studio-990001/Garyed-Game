using NUnit.Framework;
using Runtime.CH1.Pacmom;
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
        private MovementWithEyes dustMovement;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            dustObj = new GameObject("DustObj");
            dust = dustObj.AddComponent<Dust>();
            dustMovement = dustObj.AddComponent<MovementWithEyes>();
            dust.Movement = dustMovement;

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

            dustMovement.Rigid.position = Vector3.zero;
            dustMovement.SetNextDirection(new Vector2(1, 0));

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(dustMovement.Rigid.position.x > 0);
        }
    }
}