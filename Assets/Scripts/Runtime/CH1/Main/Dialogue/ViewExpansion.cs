using System;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Main.Dialogue
{
    public class ViewExpansion : DialogueViewBase
    {
        [SerializeField] private DialogueRunner _runner;
        [SerializeField] private GameObject NameTag;
        [SerializeField] private TextMeshProUGUI NameText;


        private void Awake()
        {
            _runner.onNodeStart.AddListener(NameTagCheckDisable);
        }
        
        private void NameTagCheckDisable(string nodeName)
        {
            if (NameText.text == "")
            {
                NameTag.SetActive(false);
            }
            else
            {
                NameTag.SetActive(true);
            }
        }
        
    }
}
