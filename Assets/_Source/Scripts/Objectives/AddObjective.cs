using Managers;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

namespace Objectives
{
    public class AddObjective : MonoBehaviour
    {
        #region Serialize Parameter
        
        [DisplayInspector, SerializeField] private ObjectiveScriptable newObjective;
        
        [Space]
        public UnityEvent OnBeforeTrigger;
        public UnityEvent OnAfterTrigger;
        public UnityEvent OnCompleteTrigger;
        
        #endregion

        #region Private Parameter
        
        #endregion

        #region Unity Function

        private void Awake()
        {
        }

        private void Start()
        {
        }
        
        private void OnEnable()
        {
        }
        
        private void OnDisable()
        {
        }

        private void Update()
        {
        }

        private void LateUpdate()
        {
        }

        #endregion

        #region Public Methods

        [ButtonMethod]
        public void AddQuest()
        {
            newObjective.OnBeforeTrigger += () => OnBeforeTrigger.Invoke();
            newObjective.OnAfterTrigger += () => OnAfterTrigger.Invoke();
            newObjective.OnCompleteTrigger += () => OnCompleteTrigger.Invoke();

            foreach (var objective in newObjective.objectives)
            {
                if (objective.currentValue >= objective.targetComplete)
                    continue;
                objective.isCompleted = false;
                newObjective.isComplete = false;
            }
            
            GameManager.Instance.GameEvents.AddQuest(newObjective);
        }
        
        #endregion
        
        #region Private Methods

        #endregion
    }
}