using System;
using UnityEngine;
namespace Managers
{
    public class GameEvents : MonoBehaviour
    {
        /// <summary>
        /// Update In-Game Clock
        /// </summary>
        public event Action<float, bool> onUpdateTimeUI;
        public virtual void UpdateTimeUI(float time, bool hours24 = true)
        {
            onUpdateTimeUI?.Invoke(time, hours24);
        }

        public event Action<float> onHealthUpdate;
        public virtual void HealthUpdate(float maxHealth)
        {
            onHealthUpdate?.Invoke(maxHealth);
        }
        
        public event Action<float> onStaminaUpdate;
        public virtual void StaminaUpdate(float maxHealth)
        {
            onStaminaUpdate?.Invoke(maxHealth);
        }
        
        public event Action<float> onBoredomUpdate;
        public virtual void BoredomUpdate(float maxHealth)
        {
            onBoredomUpdate?.Invoke(maxHealth);
        }

        public event Action<float> onHealthAdd;
        public virtual void AddHealth(float value)
        {
            onHealthAdd?.Invoke(value);
        }
        
        public event Action<float> onStaminaAdd;
        public virtual void AddStamina(float value)
        {
            onStaminaAdd?.Invoke(value);
        }
        
        public event Action<float> onBoredomAdd;
        public virtual void AddBoredom(float value)
        {
            onBoredomAdd?.Invoke(value);
        }
    }
}