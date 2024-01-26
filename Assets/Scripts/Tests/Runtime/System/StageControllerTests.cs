using NUnit.Framework;
using Runtime.CH1.Main.Controller;
using Runtime.CH1.Main.Map;
using Runtime.CH1.Main.Stage;
using Runtime.InGameSystem;
using Runtime.Interface;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime.System
{
    [TestFixture]
    public class StageControllerTests
    {
        
        // 비동기 처리 코드 때문에 임시 중지
        // [UnityTest]
        // public IEnumerator TestStageSwitch()
        // { 
        //     var stageControllerObj = new GameObject("StageController");
        //     IStageController stageController = stageControllerObj.AddComponent<Ch1StageController>();
        //     
        //     var stage1Obj = new GameObject("Stage1");
        //     var stage1Ob2 = new GameObject("Stage2");
        //     
        //     IStage stage1 = stage1Obj.AddComponent<Stage>();
        //     IStage stage2 = stage1Ob2.AddComponent<Stage>();
        //     
        //     stage1.StageObject = stage1Obj;
        //     stage2.StageObject = stage1Ob2;
        //     
        //     stage1Obj.transform.parent = stageControllerObj.transform;
        //     stage1Ob2.transform.parent = stageControllerObj.transform;
        //     
        //     GameObject player = new GameObject("Player");
        //     
        //     (stageController as Ch1StageController).Init(player, null, null, null);
        //     
        //     stageController.CurrentStage = stage1;
        //     
        //     stageController.StageChanger.SwitchStage(2, Vector2.zero);
        //     
        //     yield return new WaitForSeconds(2f);
        //     
        //     Assert.AreEqual(stage2, stageController.CurrentStage);
        //     
        //     GameObject.DestroyImmediate(stageControllerObj);
        //     GameObject.DestroyImmediate(stage1Obj);
        //     GameObject.DestroyImmediate(stage1Ob2);
        //     GameObject.DestroyImmediate(player);
        // }
        
        [UnityTest]
        public IEnumerator TestIStageControllerInclusion()
        {
            var stageControllerObj = new GameObject("StageController");
            IStageController stageController = stageControllerObj.AddComponent<Ch1StageController>();
            
            var stage1Obj = new GameObject("Stage1");
            IStage stage1 = stage1Obj.AddComponent<Stage>();
            
            stage1.StageObject = stage1Obj;
            stage1Obj.transform.parent = stageControllerObj.transform;
            
            (stageController as Ch1StageController).Init(null, null, null, null);
            
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(stageController, stage1.StageController);
            
            GameObject.DestroyImmediate(stageControllerObj);
            GameObject.DestroyImmediate(stage1Obj);
        }
    }
}