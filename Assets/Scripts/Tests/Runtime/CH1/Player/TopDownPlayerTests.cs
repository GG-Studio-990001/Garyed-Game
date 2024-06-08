using NUnit.Framework;
using Runtime.CH1.Main;
using Runtime.CH1.Main.Player;
using Runtime.CH1.Main.PlayerFunction;
using Runtime.ETC;
using Runtime.InGameSystem;
using Runtime.Interface;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Tests.Runtime.CH1.Player
{
    [TestFixture]
    public class TopDownPlayerTests
    {
        private GameObject _playerObject;
        private TopDownPlayer _player;
        private IMovement _movement;
        private IAnimation _animation;
        private IInteraction _interaction;
        
        [UnitySetUp]
        public IEnumerator Setup()
        {
            _playerObject = new GameObject("Player");
            Animator animator = _playerObject.AddComponent<Animator>();
            
            animator.runtimeAnimatorController = Addressables.LoadAssetAsync<AnimatorController>("Assets/Art/Animation/CH1/Rapley/Player.controller").WaitForCompletion();
            if (animator.runtimeAnimatorController == null)
            {
                Debug.LogError("Animator Controller is null");
            }
            
            _player = _playerObject.AddComponent<TopDownPlayer>();
            _player.transform.position = Vector3.zero;
            
            _movement = new TopDownMovement(5.0f, _player.transform);
            _animation = new TopDownAnimation(_player.GetComponent<Animator>(), 0.5f);
            _interaction = new TopDownInteraction(_player.transform, LayerMask.GetMask("NPC"), LayerMask.GetMask("Object"), 1f);
            
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
            _animation.SetAnimation(GlobalConst.MoveStr, new Vector2(1f, 0f)); 
            
            bool isMoving = animator.GetBool("IsMoving");
            
            yield return new WaitForFixedUpdate();
            
            Assert.IsTrue(isMoving);
        }

        /*
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
        }*/

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