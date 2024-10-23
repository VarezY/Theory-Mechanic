using System;
using MyBox;
using UnityEngine;
namespace Objectives
{
    [CreateAssetMenu(fileName = "Objective Preset", menuName = "Scriptables/Objective Preset", order = 1)]
    public class ObjectiveScriptable : ScriptableObject
    {
        public string questName;
        
        public Objective[] objectives;

        public QuestType questType;

        public Action OnBeforeTrigger;
        public Action OnAfterTrigger;
        public Action OnCompleteTrigger;

        
        [ReadOnly] public bool isComplete;
    }
}