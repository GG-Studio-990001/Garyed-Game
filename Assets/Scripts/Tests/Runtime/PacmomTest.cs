using NUnit.Framework;
using Runtime.CH1.Pacmom;
using Runtime.ETC;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    [TestFixture]
    public class PacmomTest
    {
        #region 선언
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

        private GameObject vacuumObj;
        private Vacuum vacuum;
        private GameObject vacuumParentObj;
        private Transform vacuums;

        private GameObject controllerObj;
        private PacmomGameController controller;
        #endregion

        #region SetUp & TearDown
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            rapleyObj = new GameObject("RapleyObj");
            rapleyObj.AddComponent<SpriteRenderer>();
            rapley = rapleyObj.AddComponent<Rapley>();
            rapleyMovement = rapleyObj.AddComponent<Movement>();
            rapley.movement = rapleyMovement;

            pacmomObj = new GameObject("PacmomObj");
            pacmomObj.AddComponent<SpriteRenderer>();
            pacmom = pacmomObj.AddComponent<Pacmom>();
            pacmomMovement = pacmomObj.AddComponent<Movement>();
            pacmom.movement = pacmomMovement;

            coinParentObj = new GameObject("CoinParentObj");
            coins = coinParentObj.gameObject.transform;
            coinObj = new GameObject("CoinObj");
            coinObj.transform.parent = coinParentObj.transform;
            coin = coinObj.AddComponent<Coin>();

            vacuumParentObj = new GameObject("VacuumParentObj");
            vacuums = vacuumParentObj.gameObject.transform;
            vacuumObj = new GameObject("VacuumObj");
            vacuumObj.transform.parent = vacuumParentObj.transform;
            vacuum = vacuumObj.AddComponent<Vacuum>();

            controllerObj = new GameObject("ControllerObj");
            controller = controllerObj.AddComponent<PacmomGameController>();
            controller.rapley = rapley;
            controller.pacmom = pacmom;
            controller.coins = coins;
            controller.vacuums = vacuums;

            pacmom.gameController = controller;

            yield return new WaitForFixedUpdate();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(rapleyObj);
            Object.DestroyImmediate(pacmomObj);
            Object.DestroyImmediate(coinObj);
            Object.DestroyImmediate(coinParentObj);
            Object.DestroyImmediate(vacuumObj);
            Object.DestroyImmediate(vacuumParentObj);
            Object.DestroyImmediate(controllerObj);

            yield return new WaitForFixedUpdate();
        }
        #endregion

        #region Rapley Functions
        [UnityTest]
        public IEnumerator RapleyMovement()
        {
            rapleyMovement.rigid = rapleyObj.GetComponent<Rigidbody2D>();

            rapleyMovement.rigid.position = Vector3.zero;
            rapleyMovement.SetNextDirection(new Vector2(-1, 0));
            rapleyMovement.Move();

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(rapleyMovement.rigid.position.x < 0);
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

        [UnityTest]
        public IEnumerator RapleyFlipSprite()
        {
            rapleyMovement.SetNextDirection(new Vector2(1, 0));
            rapleyMovement.Move();

            yield return new WaitForFixedUpdate();

            Assert.IsFalse(rapleyObj.GetComponent<SpriteRenderer>().flipX);
        }
        #endregion

        #region Pacmom Functions
        [UnityTest]
        public IEnumerator PacmomMovement()
        {
            pacmomMovement.rigid = rapleyObj.GetComponent<Rigidbody2D>();

            pacmomMovement.rigid.position = Vector3.zero;
            pacmomMovement.SetNextDirection(new Vector2(1, 0));
            pacmomMovement.Move();

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(pacmomMovement.rigid.position.x > 0);
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

        [UnityTest]
        public IEnumerator PacmomFlipSprite()
        {
            pacmomMovement.SetNextDirection(new Vector2(-1, 0));
            pacmomMovement.Move();

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(pacmomObj.GetComponent<SpriteRenderer>().flipX);
        }
        #endregion

        #region Collision
        [UnityTest]
        public IEnumerator PacmomRapleyCollision_1()
        {
            int pacmomLife = controller.pacmomLives;

            rapleyObj.AddComponent<CircleCollider2D>();
            rapleyObj.transform.position = Vector3.zero;
            rapleyObj.layer = LayerMask.NameToLayer(GlobalConst.PlayerStr);
            
            pacmomObj.AddComponent<CircleCollider2D>();
            pacmomObj.transform.position = Vector3.zero;
            pacmomObj.layer = LayerMask.NameToLayer(GlobalConst.PacmomStr);

            yield return new WaitForFixedUpdate();

            Assert.AreEqual(pacmomLife - 1, controller.pacmomLives);
        }

        [UnityTest]
        public IEnumerator PacmomRapleyCollision_2()
        {
            int pacmomLife = controller.pacmomLives;

            rapleyObj.AddComponent<CircleCollider2D>();
            rapleyObj.transform.position = Vector3.zero;
            rapleyObj.layer = LayerMask.NameToLayer(GlobalConst.PlayerStr);

            pacmomObj.AddComponent<CircleCollider2D>();
            pacmomObj.transform.position = Vector3.zero;
            pacmomObj.layer = LayerMask.NameToLayer(GlobalConst.PacmomStr);
            pacmom.isVacuumMode = true;

            yield return new WaitForFixedUpdate();

            Assert.AreEqual(pacmomLife, controller.pacmomLives);
        }
        #endregion
    }
}
