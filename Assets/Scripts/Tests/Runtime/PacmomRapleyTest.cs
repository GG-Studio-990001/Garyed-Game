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
        private GameObject rapleyObj;
        private Rapley rapley;
        private Movement rapleyMovement;

        private GameObject pacmomObj;
        private Pacmom pacmom;
        private Movement pacmomMovement;

        private GameObject coinObj;
        private Coin coin;
        private GameObject coinParentObj;
        private Transform coins;

        private GameObject controllerObj;
        private PacmomGameController controller;

        // 라플리 움직임
        // 라플리 동전
        // 팩맘 움직임
        // 팩맘 동전
        // 팩맘 라플리 충돌

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            rapleyObj = new GameObject("RapleyObj");
            rapley = rapleyObj.AddComponent<Rapley>();
            rapleyMovement = rapleyObj.AddComponent<Movement>();
            rapley.movement = rapleyMovement;

            pacmomObj = new GameObject("PacmomObj");
            pacmom = pacmomObj.AddComponent<Pacmom>();
            pacmomMovement = pacmomObj.AddComponent<Movement>();
            pacmom.movement = pacmomMovement;

            coinParentObj = new GameObject("CoinParentObj");
            coins = coinParentObj.gameObject.transform;
            coinObj = new GameObject("CoinObj");
            coinObj.transform.parent = coinParentObj.transform;
            coin = coinObj.AddComponent<Coin>();

            controllerObj = new GameObject("ControllerObj");
            controller = controllerObj.AddComponent<PacmomGameController>();
            controller.rapley = rapley;
            controller.coins = coins;
            controller.pacmom = pacmom;

            yield return new WaitForFixedUpdate();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(rapleyObj);
            Object.DestroyImmediate(pacmomObj);
            Object.DestroyImmediate(coinObj);
            Object.DestroyImmediate(coinParentObj);
            Object.DestroyImmediate(controllerObj);

            yield return new WaitForFixedUpdate();
        }

        #region Rapley Test
        [UnityTest]
        public IEnumerator TestRapleyMovement()
        {
            rapleyMovement.rigid = rapleyObj.GetComponent<Rigidbody2D>();

            rapleyMovement.rigid.position = Vector3.zero;
            rapleyMovement.SetNextDirection(new Vector2(-1, 0));
            rapleyMovement.Move();

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(rapleyObj.GetComponent<Movement>().rigid.position.x < 0);
        }

        [UnityTest]
        public IEnumerator TestRapleyEatCoin()
        {
            rapleyObj.AddComponent<CircleCollider2D>();
            rapleyObj.transform.position = Vector3.zero;
            rapleyObj.layer = LayerMask.NameToLayer("Player");

            BoxCollider2D coinColl = coinObj.AddComponent<BoxCollider2D>();
            coinColl.isTrigger = true;
            coinObj.transform.position = Vector3.zero;

            yield return new WaitForFixedUpdate();

            Assert.IsFalse(coinObj.activeSelf);
        }
        #endregion

        #region Pacmom Test
        [UnityTest]
        public IEnumerator TestPacmomMovement()
        {
            pacmomMovement.rigid = rapleyObj.GetComponent<Rigidbody2D>();

            pacmomMovement.rigid.position = Vector3.zero;
            pacmomMovement.SetNextDirection(new Vector2(1, 0));
            pacmomMovement.Move();

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(pacmomObj.GetComponent<Movement>().rigid.position.x > 0);
        }

        [UnityTest]
        public IEnumerator TestPacmomEatCoin()
        {
            pacmomObj.AddComponent<CircleCollider2D>();
            pacmomObj.transform.position = Vector3.zero;
            pacmomObj.layer = LayerMask.NameToLayer("Pacmom");

            BoxCollider2D coinColl = coinObj.AddComponent<BoxCollider2D>();
            coinColl.isTrigger = true;
            coinObj.transform.position = Vector3.zero;

            yield return new WaitForFixedUpdate();

            Assert.IsFalse(coinObj.activeSelf);
        }
        #endregion
    }
}
