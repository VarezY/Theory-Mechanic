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
        
        [Foldout("Current Main Quest"), ReadOnly]
        public Quest currentMainQuest = null;
        
        [Foldout("Current Side Quest"), ReadOnly]
        public List<Quest> listSideQuest;
        
        #endregion

        #region Private Parameter
        
        #endregion

        #region Unity Function

        private void Awake()
        {
        }

        private void Start()
        {
            GameManager.Instance.GameEvents.onAddQuest += GameEventsOnonAddQuest;
            
            // THIS IS A MUST TO START A NEW QUEST
            currentMainQuest.isComplete = true;
            /*var obj1 = new Objective();
            obj1.objectiveDisplayName = "Objective";
            obj1.targetComplete = 1;
            obj1.isCompleted = false;

            currentMainQuest = new Quest();
            currentMainQuest.questName = "Quest 1";
            currentMainQuest.objectives = new[]
            {
                obj1
            };

            currentMainQuest.isComplete = true;*/
        }
        private void OnEnable()
        {
        }
        
        private void OnDisable()
        {
            GameManager.Instance.GameEvents.onAddQuest -= GameEventsOnonAddQuest;
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

        private void GameEventsOnonAddQuest(Quest quest)
        {
            // Check if the quest is Main Quest or Side Quest
            switch (quest.questType)
            {
                case QuestType.MainQuest:
                    if (currentMainQuest is { isComplete: false }) return;
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
        }

        #endregion
    }
}