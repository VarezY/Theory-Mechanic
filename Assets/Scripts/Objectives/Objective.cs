using System;
using MyBox;

namespace Objectives
{
    [Serializable]
    public class Objective
    {
        public string objectiveDisplayName;

        public int targetComplete;
        public int currentValue;

        public Action OnValueChange;
        
        [ReadOnly] public bool isCompleted;
    }

    [Serializable]
    public class Quest
    {
        public string questName;
        
        public Objective[] objectives;

        public QuestType questType;

        public Action OnBeforeTrigger;
        public Action OnAfterTrigger;
        public Action OnCompleteTrigger;
        
        [ReadOnly] public bool isComplete;
    }

    [Serializable]
    public enum QuestType
    {
        MainQuest,
        SideQuest,
    }
}