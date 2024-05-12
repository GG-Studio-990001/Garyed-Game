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
        private Rapley rapley;
        private SpriteRenderer rapleySpr;
        private MovementWithFlipAndRotate rapleyMovement;

        private GameObject coinObj;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            rapleyObj = new GameObject("RapleyObj");
            rapley = rapleyObj.AddComponent<Rapley>();
            rapleySpr = rapleyObj.AddComponent<SpriteRenderer>();
            rapleyMovement = rapleyObj.AddComponent<MovementWithFlipAndRotate>();
            rapley.Movement = rapleyMovement;

            coinObj = new GameObject("CoinObj");
            coinObj.AddComponent<Coin>();

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
            rapleyMovement.SetRigidBody(rapleyObj.GetComponent<Rigidbody2D>());

            rapleyMovement.Rigid.position = Vector3.zero;
            rapleyMovement.SetNextDirection(new Vector2(-1, 0));

            yield return new WaitForSeconds(0.1f);

            Assert.IsTrue(rapleyMovement.Rigid.position.x < 0);
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