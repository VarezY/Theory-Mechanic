using Managers;
using MyBox;
using UnityEngine;
using UnityEngine.Events;
namespace Events
{
    public class TimeEvents : MonoBehaviour
    {
        #region Serialize Parameter

        [Tooltip("Reference in decimal number (1:30 pm = 13.5)"),SerializeField, MaxValue(24f), MinValue(0f)] private float triggerTime;
        [SerializeField] private bool repeating;
        [SerializeField] private UnityEvent onTimeTrigger;
        
        #endregion

        #region Private Parameter

        private bool _evoked;
        
        #endregion

        #region Unity Function

        private void Awake()
        {
        }

        private void Start()
        {
            GameManager.Instance.GameEvents.onUpdateTimeUI += OnUpdateTimeUI;
        }
        
        private void OnEnable()
        {
        }
        
        private void OnDisable()
        {
            GameManager.Instance.GameEvents.onUpdateTimeUI -= OnUpdateTimeUI;
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

        private void OnUpdateTimeUI(float time, bool type)
        {
            if (_evoked)
                return;

            if (!HasTimePassed(time, triggerTime))
                return;
           
            onTimeTrigger?.Invoke();
            
            _evoked = !repeating;
        }
        
        private static bool HasTimePassed(float currentGameTime, float targetTime)
        {
            float prevTime = Mathf.Repeat(currentGameTime - Time.deltaTime * 1f, 24f);
            return (prevTime <= targetTime && targetTime <= currentGameTime) ||
                   (prevTime > currentGameTime && 
                    (targetTime >= prevTime || targetTime <= currentGameTime));
        }

        #endregion
    }
}
