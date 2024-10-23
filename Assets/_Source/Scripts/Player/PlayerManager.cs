using System;
using System.Collections;
using Managers;
using UnityEngine;
namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        #region Serialize Parameter

        [Header("Tick Rate")]
        [Range(1, 10), SerializeField] private int tickRate = 1;
        [SerializeField] private float tickTime = 1;
        
        [Header("Player Status")]
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float healthDropRate;
        [Space]
        [SerializeField] private float maxStamina = 100;
        [SerializeField] private float staminaDropRate;
        [Space]
        [SerializeField] private float maxBoredom = 100;
        [SerializeField] private float boredomDropRate;
        
        #endregion

        #region Private Parameter

        public float CurrentHealth { get; private set; }
        public float CurrentStamina { get; private set; }
        public float CurrentBoredom { get; private set; }

        private float _tickInterval;
        private bool _isDead;
        
        #endregion

        #region Unity Function

        private void Awake()
        {
            CurrentHealth = maxHealth;
            CurrentStamina = maxStamina;
            CurrentBoredom = maxBoredom * 0.125f;
        }

        private void Start()
        {
            StartCoroutine(StatusDegradation());
            
            GameManager.Instance.GameEvents.onHealthAdd += GameEventsOnHealthAdd;
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            GameManager.Instance.GameEvents.onHealthAdd -= GameEventsOnHealthAdd;
        }

        private void Update()
        {
        }

        private void LateUpdate()
        {
        }

        private void OnValidate()
        {
            UpdateTickInterval();
        }

        #endregion

        #region Public Methods

        public void TakeDamage(float damageTaken)
        {
            if (_isDead) return;
            
            CurrentHealth -= damageTaken;
            if (CurrentHealth <= maxHealth * 0.125f)
            {
                _isDead = true;
                CurrentHealth = maxHealth * 0.125f;
            }
            
            GameManager.Instance.GameEvents.HealthUpdate(maxHealth);
        }

        public void AddHealth(float healthRestored)
        {
            GameManager.Instance.GameEvents.AddHealth(healthRestored);
            GameManager.Instance.GameEvents.HealthUpdate(maxHealth);
        }

        #endregion
        
        #region Private Methods

        private void UpdateTickInterval() => _tickInterval = tickTime / tickRate;

        private void GameEventsOnHealthAdd(float healthRestored) => CurrentHealth += healthRestored;
        
        private IEnumerator StatusDegradation()
        {
            while (!_isDead)
            {
                yield return new WaitForSeconds(_tickInterval);
                StartCoroutine(StaminaDegradation());
                StartCoroutine(BoredomAddiction());
            }
        }

        private IEnumerator StaminaDegradation()
        {
            CurrentStamina -= staminaDropRate;
            if (CurrentStamina < maxStamina * 0.125f)
            {
                CurrentStamina = maxStamina * 0.125f;
            }
            GameManager.Instance.GameEvents.StaminaUpdate(maxStamina);
            yield break;
        }
        
        private IEnumerator BoredomAddiction()
        {
            CurrentBoredom += boredomDropRate;
            if (CurrentStamina > maxBoredom)
            {
                CurrentStamina = maxBoredom;
            }
            GameManager.Instance.GameEvents.BoredomUpdate(maxBoredom);
            yield break;
        }
        
        #endregion
    }
}