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
        private MovementAndRotation pacmomMovement;
        private AI pacmomAI;

        private GameObject dustObj;
        private Dust dust;
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
            pacmomMovement = pacmomObj.AddComponent<MovementAndRotation>();
            pacmom.Movement = pacmomMovement;
            pacmomAI = pacmomObj.AddComponent<AI>();
            pacmomAI.Movement = pacmomMovement;

            dustObj = new GameObject("DustObj");
            dust = dustObj.AddComponent<Dust>();
            dustMovement = dustObj.AddComponent<MovementAndEyes>();
            dust.Movement = dustMovement;
            dustAI = dustObj.AddComponent<AI>();
            dustAI.Movement = dustMovement;

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
            pacmomMovement.Rigid.position = Vector3.zero;
            pacmomObj.layer = LayerMask.NameToLayer(GlobalConst.PacmomStr);

            pacmomAI.SetAIStronger(true);
            pacmomAI.Enemys = new Transform[1];
            pacmomAI.Enemys[0] = dustObj.transform;

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(pacmomMovement.NextDirection.x < 0);
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
            pacmomMovement.Rigid.position = Vector3.zero;
            pacmomObj.layer = LayerMask.NameToLayer(GlobalConst.PacmomStr);

            pacmomAI.SetAIStronger(false);
            pacmomAI.Enemys = new Transform[1];
            pacmomAI.Enemys[0] = dustObj.transform;

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(pacmomMovement.NextDirection.x > 0);
        }
    }
}