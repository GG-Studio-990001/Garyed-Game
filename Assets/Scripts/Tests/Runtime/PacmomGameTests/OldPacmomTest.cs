using NUnit.Framework;
using Runtime.CH1.Pacmom;
using Runtime.ETC;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime.PacmomGameTest
{
    [TestFixture]
    public class OldPacmomTest
    {
        #region 선언
        private GameObject rapleyObj;
        private SpriteRenderer rapleySpr;
        private Rapley rapley;
        private MovementAndRotation rapleyMovement;

        private GameObject pacmomObj;
        private SpriteRenderer pacmomSpr;
        private Pacmom pacmom;
        private MovementAndRotation pacmomMovement;

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

        private GameObject stepObj;
        private Step step;
        private string stepStr = "Step";
        #endregion

        #region SetUp & TearDown
        // [UnitySetUp]
        public IEnumerator SetUp()
        {
            rapleyObj = new GameObject("RapleyObj");
            rapleySpr = rapleyObj.AddComponent<SpriteRenderer>();
            rapley = rapleyObj.AddComponent<Rapley>();
            rapleyMovement = rapleyObj.AddComponent<MovementAndRotation>();
            rapley.movement = rapleyMovement;

            pacmomObj = new GameObject("PacmomObj");
            pacmomSpr = pacmomObj.AddComponent<SpriteRenderer>();
            pacmom = pacmomObj.AddComponent<Pacmom>();
            pacmomMovement = pacmomObj.AddComponent<MovementAndRotation>();
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
            // controller.rapley = rapley;
            // controller.pacmom = pacmom;
            // controller.coins = coins;
            // controller.vacuums = vacuums;

            pacmom.gameController = controller;

            stepObj = new GameObject("StepObj");
            step = stepObj.AddComponent<Step>();

            yield return new WaitForFixedUpdate();
        }

        // [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(rapleyObj);
            Object.DestroyImmediate(pacmomObj);
            Object.DestroyImmediate(coinObj);
            Object.DestroyImmediate(coinParentObj);
            Object.DestroyImmediate(vacuumObj);
            Object.DestroyImmediate(vacuumParentObj);
            Object.DestroyImmediate(controllerObj);
            Object.DestroyImmediate(stepObj);

            yield return new WaitForFixedUpdate();
        }
        #endregion

        // [UnityTest]
        public IEnumerator PacmomChaseRapley()
        {
            rapleyObj.transform.position = new Vector3(-5, 0, 0);
            rapleyObj.layer = LayerMask.NameToLayer(GlobalConst.PlayerStr);

            stepObj.AddComponent<BoxCollider2D>();
            stepObj.GetComponent<BoxCollider2D>().isTrigger = true;
            stepObj.transform.position = Vector3.zero;
            stepObj.layer = LayerMask.NameToLayer(stepStr);

            pacmom.isVacuumMode = true;
            pacmomMovement.rigid = rapleyObj.GetComponent<Rigidbody2D>();
            pacmomMovement.rigid.position = Vector3.zero;
            pacmomObj.AddComponent<CircleCollider2D>();
            pacmomObj.layer = LayerMask.NameToLayer(GlobalConst.PacmomStr);

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(pacmomMovement.nextDirection.x < 0);
        }

        // [UnityTest]
        public IEnumerator PacmomEscapeRapley()
        {
            rapleyObj.transform.position = new Vector3(-5, 0, 0);
            rapleyObj.layer = LayerMask.NameToLayer(GlobalConst.PlayerStr);

            stepObj.AddComponent<BoxCollider2D>();
            stepObj.GetComponent<BoxCollider2D>().isTrigger = true;
            stepObj.transform.position = Vector3.zero;
            stepObj.layer = LayerMask.NameToLayer(stepStr);

            pacmomMovement.rigid = rapleyObj.GetComponent<Rigidbody2D>();
            pacmomMovement.rigid.position = Vector3.zero;
            pacmomObj.AddComponent<CircleCollider2D>();
            pacmomObj.layer = LayerMask.NameToLayer(GlobalConst.PacmomStr);

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(pacmomMovement.nextDirection.x > 0);
        }

        #region Collision
        // [UnityTest]
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

        // [UnityTest]
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