using NUnit.Framework;
using Runtime.CH1.Main;
using Runtime.CH1.Main.PlayerFunction;
using Runtime.ETC;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    [TestFixture]
    public class TopDownPlayerTests
    {
        private GameObject _playerObject;
        private TopDownPlayer _player;
        private TopDownMovement _movement;
        private TopDownAnimation _animation;
        private TopDownInteraction _interaction;
        
        [UnitySetUp]
        public IEnumerator Setup()
        {
            _playerObject = new GameObject("Player");
            Animator animator = _playerObject.AddComponent<Animator>();
            
            // TODO 에섯 변경되는대로 수정
            animator.runtimeAnimatorController = Resources.Load("Sample/Player") as RuntimeAnimatorController;
            if (animator.runtimeAnimatorController == null)
            {
                Debug.LogError("Animator Controller is null");
            }
            
            _player = _playerObject.AddComponent<TopDownPlayer>();
            _player.transform.position = Vector3.zero;
            
            _movement = new TopDownMovement(5.0f, _player.transform);
            _animation = new TopDownAnimation(_player.GetComponent<Animator>(), 0.5f);
            _interaction = new TopDownInteraction(_player.transform, LayerMask.GetMask("Object"), 1f);
            
            yield return new WaitForFixedUpdate();
        }
        
        [UnityTest]
        public IEnumerator TestPlayerMovement()
        {
            Vector2 movementInput = new Vector2(1.0f, 0.0f);
            
            _movement.Move(movementInput);
            
            yield return new WaitForFixedUpdate();
            
            Assert.AreNotEqual(Vector3.zero, _player.transform.position);
        }
        
        [UnityTest]
        public IEnumerator TestPlayerAnimation()
        {
            Animator animator = _playerObject.GetComponent<Animator>();
            _animation.SetMovementAnimation(PlayerState.Move,new Vector2(1f, 0f)); 
            
            bool isMoving = animator.GetBool("IsMoving");
            
            yield return new WaitForFixedUpdate();
            
            Assert.IsTrue(isMoving);
        }
        
        [UnityTest]
        public IEnumerator TestPlayerInteraction()
        {
            GameObject interactionObject = new GameObject("InteractionTestObject1");
            interactionObject.AddComponent<CircleCollider2D>();
            interactionObject.AddComponent<NpcInteraction>();
            interactionObject.layer = LayerMask.NameToLayer("Object");
            
            interactionObject.transform.position = new Vector3(1.0f, 0.0f, 0.0f);
            
            yield return new WaitForFixedUpdate();
            
            Assert.IsTrue(_interaction.Interact(Vector2.right));
            
            Object.DestroyImmediate(interactionObject);
        }
        
        [UnityTest]
        public IEnumerator TestPlayerInteractionFail()
        {
            GameObject interactionObject = new GameObject("InteractionTestObject2");
            interactionObject.AddComponent<CircleCollider2D>();
            interactionObject.AddComponent<NpcInteraction>();
            interactionObject.layer = LayerMask.NameToLayer("Object");
            
            interactionObject.transform.position = new Vector3(1.0f, 0.0f, 0.0f);
            
            yield return new WaitForFixedUpdate();
            
            Assert.IsFalse(_interaction.Interact(Vector2.left));
            
            Object.DestroyImmediate(interactionObject);
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.Destroy(_playerObject);

            _interaction = null;
            _movement = null;
            _animation = null;
            
            yield return new WaitForFixedUpdate();
        }
    }
}