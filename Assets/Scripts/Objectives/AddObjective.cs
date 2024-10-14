using Managers;
using MyBox;
using UnityEngine;

namespace Objectives
{
    public class AddObjective : MonoBehaviour
    {
        #region Serialize Parameter
        
        [SerializeField] private Quest newQuest = null;
        
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
            GameManager.Instance.GameEvents.AddQuest(newQuest);
        }
        
        #endregion
        
        #region Private Methods

        #endregion
    }
}