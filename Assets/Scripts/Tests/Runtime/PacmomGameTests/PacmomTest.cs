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
        private SpriteRenderer pacmomSpr;
        private Pacmom pacmom;
        private Movement pacmomMovement;

        private GameObject coinObj;
        private Coin coin;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            pacmomObj = new GameObject("PacmomObj");
            pacmomSpr = pacmomObj.AddComponent<SpriteRenderer>();
            pacmom = pacmomObj.AddComponent<Pacmom>();
            pacmomMovement = pacmomObj.AddComponent<Movement>();
            pacmom.movement = pacmomMovement;

            coinObj = new GameObject("CoinObj");
            coin = coinObj.AddComponent<Coin>();

            yield return new WaitForFixedUpdate();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(pacmomObj);
            Object.DestroyImmediate(coinObj);

            yield return new WaitForFixedUpdate();
        }

        [UnityTest]
        public IEnumerator PacmomMovement()
        {
            pacmomMovement.rigid = pacmomObj.GetComponent<Rigidbody2D>();

            pacmomMovement.rigid.position = Vector3.zero;
            pacmomMovement.SetNextDirection(new Vector2(1, 0));

            yield return new WaitForFixedUpdate();

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

            BoxCollider2D coinColl = coinObj.AddComponent<BoxCollider2D>();
            coinColl.isTrigger = true;
            coinObj.transform.position = Vector3.zero;

            yield return new WaitForFixedUpdate();

            Assert.IsFalse(coinObj.activeSelf);
        }
    }
}
