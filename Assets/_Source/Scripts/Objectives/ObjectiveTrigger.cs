using Managers;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;
namespace Objectives
{
    public class ObjectiveTrigger : MonoBehaviour
    {
        #region Serialize Parameter
        
        [FormerlySerializedAs("newObjective")]
        [DisplayInspector, SerializeField] private ObjectiveScriptable targetQuest;

        [SerializeField] private int objectiveIndex;
        [SerializeField] private int objectiveValue;
        
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

        

        #endregion
        
        #region Private Methods

        [ButtonMethod]
        private void UpdateObjectiveQuest()
        {
            GameManager.Instance.GameEvents.UpdateObjective(targetQuest, objectiveIndex, objectiveValue);
        }

        #endregion
    }
}