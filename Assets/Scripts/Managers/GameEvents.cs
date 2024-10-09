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
    }
}