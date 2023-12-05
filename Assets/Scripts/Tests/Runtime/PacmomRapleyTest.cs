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

        private GameObject vacuumObj;
        private Vacuum vacuum;
        private GameObject vacuumParentObj;
        private Transform vacuums;

        private GameObject controllerObj;
        private PacmomGameController controller;

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
