using NUnit.Framework;
using Runtime.CH1.Pacmom;
using Runtime.ETC;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime.PacmomGameTest
{
    [TestFixture]
    public class AITest
    {
        private GameObject pacmomObj;
        private Pacmom pacmom;
        private SpriteRenderer pacmomSpr;
        private MovementAndRotation pacmomMovement;
        private AI pacmomAI;

        private GameObject dustObj;
        private Dust dust;
        private SpriteRenderer dustSpr;
        private MovementAndEyes dustMovement;
        private AI dustAI;

        private GameObject stepObj;
        private Step step;
        private readonly string stepStr = "Step";

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            pacmomObj = new GameObject("PacmomObj");
            pacmom = pacmomObj.AddComponent<Pacmom>();
            pacmomSpr = pacmomObj.AddComponent<SpriteRenderer>();
            pacmomMovement = pacmomObj.AddComponent<MovementAndRotation>();
            pacmom.movement = pacmomMovement;
            pacmomAI = pacmomObj.AddComponent<AI>();
            pacmomAI.movement = pacmomMovement;

            dustObj = new GameObject("DustObj");
            dust = dustObj.AddComponent<Dust>();
            dustSpr = dustObj.AddComponent<SpriteRenderer>();
            dustMovement = dustObj.AddComponent<MovementAndEyes>();
            dust.movement = dustMovement;
            dustAI = dustObj.AddComponent<AI>();
            dustAI.movement = dustMovement;

            stepObj = new GameObject("StepObj");
            step = stepObj.AddComponent<Step>();

            yield return new WaitForFixedUpdate();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(dustObj);
            Object.DestroyImmediate(pacmomObj);
            Object.DestroyImmediate(stepObj);

            yield return new WaitForFixedUpdate();
        }

        [UnityTest]
        public IEnumerator ChaseEnemy()
        {
            dustObj.transform.position = new Vector3(-5, 0, 0);

            stepObj.AddComponent<BoxCollider2D>().isTrigger = true;
            stepObj.transform.position = Vector3.zero;
            stepObj.layer = LayerMask.NameToLayer(stepStr);

            pacmomObj.AddComponent<CircleCollider2D>();
            pacmomMovement.SetRigidBody(pacmomObj.GetComponent<Rigidbody2D>());
            pacmomMovement.rigid.position = Vector3.zero;
            pacmomObj.layer = LayerMask.NameToLayer(GlobalConst.PacmomStr);

            pacmomAI.SetStronger(true);
            pacmomAI.enemys = new Transform[1];
            pacmomAI.enemys[0] = dustObj.transform;

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(pacmomMovement.nextDirection.x < 0);
        }
        
        [UnityTest]
        public IEnumerator RunAwayFromEnemy()
        {
            dustObj.transform.position = new Vector3(-5, 0, 0);

            stepObj.AddComponent<BoxCollider2D>().isTrigger = true;
            stepObj.transform.position = Vector3.zero;
            stepObj.layer = LayerMask.NameToLayer(stepStr);

            pacmomObj.AddComponent<CircleCollider2D>();
            pacmomMovement.SetRigidBody(pacmomObj.GetComponent<Rigidbody2D>());
            pacmomMovement.rigid.position = Vector3.zero;
            pacmomObj.layer = LayerMask.NameToLayer(GlobalConst.PacmomStr);

            pacmomAI.SetStronger(false);
            pacmomAI.enemys = new Transform[1];
            pacmomAI.enemys[0] = dustObj.transform;

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(pacmomMovement.nextDirection.x > 0);
        }

        /*
        [UnityTest]
        public IEnumerator RapleyEatPacmom()
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
        */
    }
}