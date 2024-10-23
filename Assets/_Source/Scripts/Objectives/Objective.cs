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
        [ReadOnly] public ObjectiveUI ui;

        [ReadOnly] public bool isCompleted;
    }

    [Serializable]
    public enum QuestType
    {
        MainQuest,
        SideQuest,
    }
}