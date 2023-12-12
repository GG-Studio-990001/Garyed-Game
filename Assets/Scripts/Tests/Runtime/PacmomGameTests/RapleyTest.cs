using NUnit.Framework;
using Runtime.CH1.Pacmom;
using Runtime.ETC;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime.PacmomGameTest
{
    [TestFixture]
    public class RapleyTest
    {
        private GameObject rapleyObj;
        private SpriteRenderer rapleySpr;
        private Rapley rapley;
        private Movement rapleyMovement;

        private GameObject coinObj;
        private Coin coin;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            rapleyObj = new GameObject("RapleyObj");
            rapleySpr = rapleyObj.AddComponent<SpriteRenderer>();
            rapley = rapleyObj.AddComponent<Rapley>();
            rapleyMovement = rapleyObj.AddComponent<Movement>();
            rapley.movement = rapleyMovement;

            coinObj = new GameObject("CoinObj");
            coin = coinObj.AddComponent<Coin>();

            yield return new WaitForFixedUpdate();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(rapleyObj);
            Object.DestroyImmediate(coinObj);

            yield return new WaitForFixedUpdate();
        }

        [UnityTest]
        public IEnumerator RapleyMovement()
        {
            rapleyMovement.rigid = rapleyObj.GetComponent<Rigidbody2D>();

            rapleyMovement.rigid.position = Vector3.zero;
            rapleyMovement.SetNextDirection(new Vector2(-1, 0));

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(rapleyMovement.rigid.position.x < 0);
        }

        [UnityTest]
        public IEnumerator RapleyFlipSprite()
        {
            rapleyMovement.SetNextDirection(new Vector2(1, 0));

            yield return new WaitForFixedUpdate();

            Assert.IsFalse(rapleySpr.flipX);
        }

        [UnityTest]
        public IEnumerator RapleyEatCoin()
        {
            rapleyObj.AddComponent<CircleCollider2D>();
            rapleyObj.transform.position = Vector3.zero;
            rapleyObj.layer = LayerMask.NameToLayer(GlobalConst.PlayerStr);

            BoxCollider2D coinColl = coinObj.AddComponent<BoxCollider2D>();
            coinColl.isTrigger = true;
            coinObj.transform.position = Vector3.zero;

            yield return new WaitForFixedUpdate();

            Assert.IsFalse(coinObj.activeSelf);
        }
    }
}