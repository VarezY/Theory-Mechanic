using System;
using Audio;
using UI;
using UnityEngine;
namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        // public PlayerController PlayerController;
        
        public UIController UIController { get; private set; }
        public AudioController AudioController { get; private set; }
        public GameEvents GameEvents { get; private set; }


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            GameEvents = GetComponent<GameEvents>();

            UIController = GetComponentInChildren<UIController>();
            AudioController = GetComponentInChildren<AudioController>();

        }

        public void TestDebug(string message)
        {
            print($"Object Click {message}");
        }
    }
}
