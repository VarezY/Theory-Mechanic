using System.Collections.Generic;
using System.Linq;
using Managers;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objectives
{
    public class ObjectivesManager : MonoBehaviour
    {
        #region Serialize Parameter
        
        [Foldout("Current Main Quest"), DisplayInspector, ReadOnly]
        public ObjectiveScriptable currentMainQuest = null;
        
        [Foldout("Current Side Quest"), DisplayInspector, ReadOnly]
        public List<ObjectiveScriptable> listSideQuest;
        
        #endregion

        #region Private Parameter
        
        #endregion

        #region Unity Function

        private void Awake()
        {
        }

        private void Start()
        {
            GameManager.Instance.GameEvents.onAddQuest += GameEventsOnAddQuest;
            GameManager.Instance.GameEvents.onUpdateObjective += GameEventOnUpdateQuest;
        }
        private void OnEnable()
        {
        }
        
        private void OnDisable()
        {
            GameManager.Instance.GameEvents.onAddQuest -= GameEventsOnAddQuest;
            GameManager.Instance.GameEvents.onUpdateObjective -= GameEventOnUpdateQuest;
        }

        private void Update()
        {
        }

        private void LateUpdate()
        {
        }

        #endregion

        #region Public Methods

        #endregion
        
        #region Private Methods

        private void GameEventsOnAddQuest(ObjectiveScriptable quest)
        {
            // Check if the quest is Main Quest or Side Quest
            switch (quest.questType)
            {
                case QuestType.MainQuest:
                    if (currentMainQuest is { isComplete: false }) return;
                    if (currentMainQuest is not null)
                    {
                        foreach (var objective in currentMainQuest.objectives)
                        {
                            Destroy(objective.ui.gameObject);
                        }    
                    }
                    currentMainQuest = quest;
                    GameManager.Instance.GameEvents.AddObjective(quest.objectives, true);
                    break;
                case QuestType.SideQuest:
                    listSideQuest.Add(quest);
                    GameManager.Instance.GameEvents.AddObjective(quest.objectives, false);
                    break;
                default:
                    break;
            }
            GameManager.Instance.AudioController.PlayAudioNewObjective();
        }

        private static void GameEventOnUpdateQuest(ObjectiveScriptable quest, int objectiveIndex, int objectiveValue)
        {
            if (objectiveIndex < 0 || objectiveIndex >= quest.objectives.Length) return;
            
            var questObjective = quest.objectives[objectiveIndex];
            if (questObjective.isCompleted) return;
            
            questObjective.currentValue += objectiveValue;
            
            // Check if the objectives has completed or not
            if (questObjective.currentValue >= questObjective.targetComplete)
            {
                questObjective.isCompleted = true;
                questObjective.ui.ShowCheckmark(true);
            }
            
            questObjective.OnValueChange?.Invoke();
            questObjective.ui.ObjectiveText(string.Format(questObjective.objectiveDisplayName, questObjective.currentValue, questObjective.targetComplete));
            
            CheckQuestCompleted(quest);
        }

        private static void CheckQuestCompleted(ObjectiveScriptable quest)
        {
            int completion = quest.objectives.Count(objective => objective.isCompleted);
            if (quest.objectives.Length != completion) return;
            quest.OnCompleteTrigger.Invoke();
            quest.isComplete = true;
            
            GameManager.Instance.AudioController.PlayAudioCompleteObjective();
            
            foreach (var objective in quest.objectives)
            {
                objective.ui.ObjectiveCompleated();
                objective.ui.BlinkAnimation();
                if (quest.questType == QuestType.SideQuest)
                {
                    Destroy(objective.ui.gameObject, 15f);
                }
            }
        }

        #endregion
    }
}