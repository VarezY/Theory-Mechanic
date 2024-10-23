using System;
using Item;
using MyBox;
using UnityEngine;
using UnityEngine.Events;
namespace Events
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEvents : MonoBehaviour
    {
        [System.Flags]
        private enum TriggerType
        {
            None = 0,
            Enter = 1,
            Stay = 2,
            Exit = 4,
        }
        
        #region Serialize Parameter

        [SerializeField] private TriggerType triggerType;

        [Space]
        [ConditionalField(nameof(triggerType), false, TriggerType.Enter),SerializeField] private bool repeatingEnter;
        [ConditionalField(nameof(triggerType), false, TriggerType.Enter), SerializeField] private UnityEvent onTriggerObjectEnter;
        
        [Space]
        [ConditionalField(nameof(triggerType), false, TriggerType.Stay),SerializeField] private bool repeatingStay;
        [ConditionalField(nameof(triggerType), false, TriggerType.Stay), SerializeField] private UnityEvent onTriggerObjectStay;
        
        [Space]
        [ConditionalField(nameof(triggerType), false, TriggerType.Exit),SerializeField] private bool repeatingExit;
        [ConditionalField(nameof(triggerType), false, TriggerType.Exit), SerializeField] private UnityEvent onTriggerObjectExit;

        #endregion

        #region Private Parameter

        private bool _entered;
        private bool _exited;
        
        #endregion

        #region Unity Function

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
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

        #region Trigger

        private void OnTriggerEnter(Collider other)
        {
            if (_entered)  
                return;
            
            if (!triggerType.HasFlag(TriggerType.Enter))
                return;

            if (!other.CompareTag("Player"))
                return;

            onTriggerObjectEnter?.Invoke();
            _entered = !repeatingEnter;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!triggerType.HasFlag(TriggerType.Stay))
                return;

            if (!other.CompareTag("Player"))
                return;

            onTriggerObjectStay?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (_exited)  
                return;
            
            if (!triggerType.HasFlag(TriggerType.Exit))
                return;

            if (!other.CompareTag("Player"))
                return;

            onTriggerObjectExit?.Invoke();
            _exited = !repeatingExit;
        }

        #endregion
        
        #region Public Methods

        

        #endregion
        
        #region Private Methods

        

        #endregion
    }
}
