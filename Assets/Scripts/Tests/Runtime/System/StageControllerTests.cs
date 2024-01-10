using NUnit.Framework;
using Runtime.CH1.Main.Controller;
using Runtime.CH1.Main.Map;
using Runtime.Interface;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime.System
{
    [TestFixture]
    public class StageControllerTests
    {
        [UnityTest]
        public IEnumerator TestStageSwitch()
        { 
            var stageControllerObj = new GameObject("StageController");
            IStageController stageController = stageControllerObj.AddComponent<StageController>();
            
            var stage1Obj = new GameObject("Stage1");
            var stage1Ob2 = new GameObject("Stage2");
            
            IStage stage1 = stage1Obj.AddComponent<Stage>();
            IStage stage2 = stage1Ob2.AddComponent<Stage>();
            
            stage1.StageObject = stage1Obj;
            stage2.StageObject = stage1Ob2;
            
            stage1Obj.transform.parent = stageControllerObj.transform;
            stage1Ob2.transform.parent = stageControllerObj.transform;
            
            GameObject player = new GameObject("Player");
            
            (stageController as StageController).Init(player, null);
            
            stageController.CurrentStage = stage1;
            
            stageController.SwitchStage(2, Vector2.zero);
            
            yield return new WaitForEndOfFrame();
            
            Assert.AreEqual(stage2, stageController.CurrentStage);
            
            GameObject.DestroyImmediate(stageControllerObj);
            GameObject.DestroyImmediate(stage1Obj);
            GameObject.DestroyImmediate(stage1Ob2);
            GameObject.DestroyImmediate(player);
        }
        
        [UnityTest]
        public IEnumerator TestIStageControllerInclusion()
        {
            var stageControllerObj = new GameObject("StageController");
            IStageController stageController = stageControllerObj.AddComponent<StageController>();
            
            var stage1Obj = new GameObject("Stage1");
            IStage stage1 = stage1Obj.AddComponent<Stage>();
            
            stage1.StageObject = stage1Obj;
            stage1Obj.transform.parent = stageControllerObj.transform;
            
            (stageController as StageController).Init(null, null);
            
            yield return new WaitForEndOfFrame();
            
            Assert.AreEqual(stageController, stage1.StageController);
            
            GameObject.DestroyImmediate(stageControllerObj);
            GameObject.DestroyImmediate(stage1Obj);
        }
    }
}