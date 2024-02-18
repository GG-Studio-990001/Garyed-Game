using NUnit.Framework;
using Runtime.CH1.Main.Stage;
using Runtime.Interface;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime.System
{
    [TestFixture]
    public class StageControllerTests
    {
        
         // [UnityTest]
         // public IEnumerator TestStageSwitch()
         // { 
         //     // //Arrange
         //     // var stageControllerObj = new GameObject("StageController");
         //     // Ch1StageController stageController = stageControllerObj.AddComponent<Ch1StageController>();
         //     //
         //     // var stage1Obj = new GameObject("Stage1");
         //     // var stage1Ob2 = new GameObject("Stage2");
         //     //
         //     // //IStage stage1 = stage1Obj.AddComponent<Stage>();
         //     // //IStage stage2 = stage1Ob2.AddComponent<Stage>();
         //     //
         //     // stage1.StageObject = stage1Obj;
         //     // stage2.StageObject = stage1Ob2;
         //     //
         //     // stage1Obj.transform.parent = stageControllerObj.transform;
         //     // stage1Ob2.transform.parent = stageControllerObj.transform;
         //     //
         //     // GameObject player = new GameObject("Player");
         //     //
         //     // //Action
         //     // //stageController.Init(player, null, null, null);
         //     //
         //     // //stageController.CurrentStage = stage1;
         //     // var task = stageController.StageChanger.SwitchStage(2, Vector2.zero);
         //     //
         //     // yield return new WaitForFixedUpdate();
         //     //
         //     // yield return new WaitUntil(() => task.IsCompleted);
         //     //
         //     // //Debug.Log($"{stageController.CurrentStage} Stage Changed Checker!!!!!!!!!");
         //     //
         //     // //Assert.AreEqual(stage2, stageController.CurrentStage);
         //     //
         //     // GameObject.DestroyImmediate(stageControllerObj);
         //     // GameObject.DestroyImmediate(stage1Obj);
         //     // GameObject.DestroyImmediate(stage1Ob2);
         //     // GameObject.DestroyImmediate(player);
         // }
        
        // [UnityTest]
        // public IEnumerator TestIStageControllerInclusion()
        // {
        //     // var stageControllerObj = new GameObject("StageController");
        //     // Ch1StageController stageController = stageControllerObj.AddComponent<Ch1StageController>();
        //     //
        //     // var stage1Obj = new GameObject("Stage1");
        //     // IStage stage1 = stage1Obj.AddComponent<Stage>();
        //     //
        //     // stage1.StageObject = stage1Obj;
        //     // stage1Obj.transform.parent = stageControllerObj.transform;
        //     //
        //     // //stageController.Init(null, null, null, null);
        //     //
        //     // yield return new WaitForFixedUpdate();
        //     //
        //     // Assert.AreEqual(stageController, stage1.StageController);
        //     //
        //     // GameObject.DestroyImmediate(stageControllerObj);
        //     // GameObject.DestroyImmediate(stage1Obj);
        // }
    }
}