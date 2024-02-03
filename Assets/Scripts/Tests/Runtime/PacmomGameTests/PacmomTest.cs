using NUnit.Framework;
using Runtime.CH1.Pacmom;
using Runtime.ETC;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime.PacmomGameTest
{
    [TestFixture]
    public class PacmomTest
    {
        private GameObject pacmomObj;
        private Pacmom pacmom;
        private SpriteRenderer pacmomSpr;
        private MovementAndRotation pacmomMovement;

        private GameObject coinObj;
        private Coin coin;

        private GameObject vacuumObj;
        private Vacuum vacuum;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            pacmomObj = new GameObject("PacmomObj");
            pacmom = pacmomObj.AddComponent<Pacmom>();
            pacmomSpr = pacmomObj.AddComponent<SpriteRenderer>();
            pacmomMovement = pacmomObj.AddComponent<MovementAndRotation>();
            pacmom.SetMovement(pacmomMovement);

            coinObj = new GameObject("CoinObj");
            coin = coinObj.AddComponent<Coin>();

            vacuumObj = new GameObject("VacuumObj");
            vacuum = vacuumObj.AddComponent<Vacuum>();

            yield return new WaitForFixedUpdate();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(pacmomObj);
            Object.DestroyImmediate(coinObj);
            Object.DestroyImmediate(vacuumObj);

            yield return new WaitForFixedUpdate();
        }

        [UnityTest]
        public IEnumerator PacmomMovement()
        {
            pacmomMovement.SetRigidBody(pacmomObj.GetComponent<Rigidbody2D>());

            pacmomMovement.rigid.position = Vector3.zero;
            pacmomMovement.SetNextDirection(new Vector2(1, 0));

            yield return new WaitForSeconds(0.1f);

            Debug.Log(pacmomMovement.rigid.position.x);
            Assert.IsTrue(pacmomMovement.rigid.position.x > 0);
        }

        [UnityTest]
        public IEnumerator PacmomFlipSprite()
        {
            pacmomMovement.SetNextDirection(new Vector2(-1, 0));

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(pacmomSpr.flipX);
        }

        [UnityTest]
        public IEnumerator PacmomRotate()
        {
            pacmomMovement.SetNextDirection(new Vector2(0, 1));

            yield return new WaitForFixedUpdate();

            Assert.AreNotEqual(pacmomObj.transform.rotation.z, 0);
        }

        [UnityTest]
        public IEnumerator PacmomEatCoin()
        {
            pacmomObj.AddComponent<CircleCollider2D>();
            pacmomObj.transform.position = Vector3.zero;
            pacmomObj.layer = LayerMask.NameToLayer(GlobalConst.PacmomStr);

            coinObj.AddComponent<BoxCollider2D>().isTrigger = true;
            coinObj.transform.position = Vector3.zero;

            yield return new WaitForFixedUpdate();
        
            Assert.IsFalse(coinObj.activeSelf);
        }

        [UnityTest]
        public IEnumerator PacmomEatVacuum()
        {
            pacmomObj.AddComponent<CircleCollider2D>();
            pacmomObj.transform.position = Vector3.zero;
            pacmomObj.layer = LayerMask.NameToLayer(GlobalConst.PacmomStr);

            vacuumObj.AddComponent<BoxCollider2D>().isTrigger = true;
            vacuumObj.transform.position = Vector3.zero;

            yield return new WaitForFixedUpdate();

            Assert.IsFalse(vacuumObj.activeSelf);
        }
    }
}
