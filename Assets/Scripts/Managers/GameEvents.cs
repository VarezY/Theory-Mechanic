using System;
using System.Collections.Generic;
using Objectives;
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

        #region Player Status

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

        #endregion

        public event Action<Quest> onAddQuest;
        public virtual void AddQuest(Quest quest)
        {
            onAddQuest?.Invoke(quest);
        }
        
        public event Action<Quest, List<Quest>> onUpdateQuest;
        public virtual void UpdateQuest(Quest mainQuest, List<Quest> sideQuests)
        {
            onUpdateQuest?.Invoke(mainQuest, sideQuests);
        }        
        
        public event Action<Objective[], bool> onAddObjective;
        public virtual void AddObjective(Objective[] objective, bool isMainQuest)
        {
            onAddObjective?.Invoke(objective, isMainQuest);
        }
        
        public event Action<Objective, int> onUpdateObjective;
        public virtual void UpdateObjective(Objective objective, int value)
        {
            onUpdateObjective?.Invoke(objective, value);
        }

        
    }
}