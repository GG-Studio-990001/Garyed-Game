using NUnit.Framework;
using Runtime.CH1.SubB;
using Runtime.ETC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime.CH1.Sub_B
{
    [TestFixture]
    public class ThreeMatchPuzzleTests
    {
        private GameObject _controllerObject;
        private ThreeMatchPuzzleController _controller;
        private List<Jewelry> _jewelries;
        
        [UnitySetUp]
        public IEnumerator Setup()
        {
            _controllerObject = new GameObject("Controller");
            _controller = _controllerObject.AddComponent<ThreeMatchPuzzleController>();
            
            _jewelries = new List<Jewelry>();
            
            for (int i = 0; i < 3; i++)
            {
                _jewelries[i] = new GameObject($"Jewelry{i}").AddComponent<Jewelry>();
                _jewelries[i].transform.parent = _controller.transform;
                _jewelries[i].transform.position = new Vector3(i, 0, 0f);
                _jewelries[i].JewelryType = JewelryType.None;
            }
            
            _controller.Jewelries = _jewelries;
            
            yield return new WaitForFixedUpdate();
        }
        
        [UnityTest]
        public IEnumerator TestCheckMatchingSuccess()
        {
            _jewelries[0].JewelryType = JewelryType.A;
            _jewelries[1].JewelryType = JewelryType.A;
            _jewelries[2].JewelryType = JewelryType.A;
            
            _controller.CheckMatching();
            
            yield return new WaitForFixedUpdate();
            
            Assert.IsTrue(_controller.IsClear);
        }
        
        [UnityTest]
        public IEnumerator TestCheckMatchingFail()
        {
            _jewelries[0].JewelryType = JewelryType.A;
            _jewelries[1].JewelryType = JewelryType.B;
            _jewelries[2].JewelryType = JewelryType.A;
            
            _controller.CheckMatching();
            
            yield return new WaitForFixedUpdate();
            
            Assert.IsFalse(_controller.IsClear);
        }
        
        [UnityTest]
        public IEnumerator TestValidateMovementSuccess()
        {
            _jewelries[0].JewelryType = JewelryType.A;
            _jewelries[1].JewelryType = JewelryType.B;
            _jewelries[2].JewelryType = JewelryType.A;
            
            Assert.IsTrue(_controller.ValidateMovement(_jewelries[0], Vector2.up));
            
            yield return new WaitForFixedUpdate();
        }
        
        [UnityTest]
        public IEnumerator TestValidateMovementFail()
        {
            _jewelries[0].JewelryType = JewelryType.A;
            _jewelries[1].JewelryType = JewelryType.B;
            _jewelries[2].JewelryType = JewelryType.A;
            
            Assert.IsFalse(_controller.ValidateMovement(_jewelries[0], Vector2.right));
            
            yield return new WaitForFixedUpdate();
        }

        [UnityTest]
        public IEnumerator TestPuzzleReset()
        {
            _jewelries[0].transform.position = new Vector3(1, 1, 0);
            
            _controller.PuzzleReset();
            
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(new Vector3(0, 0, 0), _jewelries[0].transform.position);
        }

        [UnityTearDown]
        public IEnumerator Teardown()
        {
            Object.Destroy(_controllerObject);
            
            yield return new WaitForFixedUpdate();
        }
    }
}