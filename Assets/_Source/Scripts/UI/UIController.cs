using System;
using System.Collections.Generic;
using System.Linq;
using Item;
using Managers;
using MyBox;
using Objectives;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    public class UIController : MonoBehaviour
    {
        #region Serialize Parameter

        [Header("Interact Button")]
        [SerializeField] private GameObject buttonPrompt;
        [SerializeField] private Transform buttonRootLocation;

        [Header("Slider Status")]
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider staminaSlider;
        [SerializeField] private Slider boredomSlider;
        
        [Header("Clock")]
        [SerializeField] private TMP_Text clockText;
        
        [Header("Quest")]
        [SerializeField] private GameObject questPanel;
        [SerializeField] private Transform questRootLocation;
        [SerializeField] private GameObject mainQuestPanel;
        [SerializeField] private GameObject sideQuestPanel;
        
        #endregion

        #region Private Parameter

        public Button SelectedButton { get; private set; }
        private int _indexButton;
        
        #endregion

        #region Unity Function

        private void Awake()
        {
        }

        private void Start()
        {
            GameManager.Instance.GameEvents.onAddObjective += GameEventsOnAddObjective;
            
            // UI
            GameManager.Instance.GameEvents.onUpdateTimeUI += GameEventsOnUpdateTimeUI;
            GameManager.Instance.GameEvents.onHealthUpdate += GameEventOnHealthUpdate;
            GameManager.Instance.GameEvents.onStaminaUpdate += GameEventOnStaminaUpdate;
            GameManager.Instance.GameEvents.onBoredomUpdate += GameEventOnBoredomUpdate;
        }

        private void OnEnable()
        {
        }
        
        private void OnDisable()
        {
            GameManager.Instance.GameEvents.onAddObjective -= GameEventsOnAddObjective;

            // UI
            GameManager.Instance.GameEvents.onUpdateTimeUI -= GameEventsOnUpdateTimeUI;
            GameManager.Instance.GameEvents.onHealthUpdate -= GameEventOnHealthUpdate;
            GameManager.Instance.GameEvents.onStaminaUpdate -= GameEventOnStaminaUpdate;
            GameManager.Instance.GameEvents.onBoredomUpdate -= GameEventOnBoredomUpdate;
        }

        private void Update()
        {
            
        }

        private void LateUpdate()
        {
        }

        #endregion

        #region Public Methods

        public void GenerateButtonPrompt(ItemController targetItem)
        {
            if (targetItem.eventsList.IsNullOrEmpty()) return;

            var i = 1;
            foreach (var interactEvent in targetItem.eventsList)
            {
                var tempButton = Instantiate(buttonPrompt, buttonRootLocation);
                if (!interactEvent.interactName.IsNullOrEmpty())
                {
                    tempButton.name = interactEvent.interactName;
                    tempButton.GetComponentInChildren<TMP_Text>().text = interactEvent.interactName;
                }
                else
                {
                    tempButton.name = $"Interact Button";
                    tempButton.GetComponentInChildren<TMP_Text>().text = $"Interact {++i}";
                }
                tempButton.GetComponent<Button>().onClick.AddListener(() => interactEvent.onInteract.Invoke());
               
                if (SelectedButton)
                    continue;
                SelectedButton = tempButton.GetComponent<Button>();
                SelectedButton.GetComponent<TextPrompt>().SelectIcon(true);
                _indexButton = 0;
            }
        }

        public void SelectButtonPrompt(int index)
        {
            var buttonList = buttonRootLocation.GetComponentsInChildren<Button>();
            if (buttonList.IsNullOrEmpty())
                return;
            
            SelectedButton.GetComponent<TextPrompt>().SelectIcon(false);
            switch (index)
            {
                case > 0:
                    if (_indexButton - 1 < 0)
                        break;
                    SelectedButton = buttonList[--_indexButton];
                    GameManager.Instance.AudioController.PlayAudioSelect();
                    break;
                case < 0:
                    if (_indexButton + 1 > buttonList.Length - 1)
                        break;
                    SelectedButton = buttonList[++_indexButton];
                    GameManager.Instance.AudioController.PlayAudioSelect();
                    break;
                default:
                    return;
            }
            
            SelectedButton.GetComponent<TextPrompt>().SelectIcon(true);
        }

        public void ClearButtonPrompt()
        {
            foreach (Transform child in buttonRootLocation)
            {
                Destroy(child.gameObject);
            }
        }

        #endregion
        
        #region Private Methods
        
        private void GameEventsOnUpdateTimeUI(float time, bool type)
        {
            time = Mathf.Repeat(time, 24f);
            
            int hours = Mathf.FloorToInt(time);
            int minutes = Mathf.RoundToInt((time - hours) * 60);

            if (minutes == 60)
            {
                hours = (hours + 1) % 24;
                minutes = 0;
            }
            
            string result;
            switch (type)
            {
                case true:
                    result = $"{hours:D2}:{minutes:D2}";
                    break;
                case false:
                    string time12 = hours switch
                    {
                        0 => $"12:{minutes:D2} AM",
                        < 12 => $"{hours}:{minutes:D2} AM",
                        12 => $"12:{minutes:D2} PM",
                        _ => $"{hours - 12}:{minutes:D2} PM"
                    };
                    
                    result = time12;
                    break;
            }

            clockText.text = result;
        }

        private void GameEventOnHealthUpdate(float maxHealth)
        {
            float percentage = GameManager.Instance.PlayerManager.CurrentHealth / maxHealth;
            healthSlider.value = percentage;
        }
        
        private void GameEventOnStaminaUpdate(float maxStamina)
        {
            float percentage = GameManager.Instance.PlayerManager.CurrentStamina / maxStamina;
            staminaSlider.value = percentage;
        }

        private void GameEventOnBoredomUpdate(float maxBoredom)
        {
            float percentage = GameManager.Instance.PlayerManager.CurrentBoredom / maxBoredom;
            boredomSlider.value = percentage;
        }
        
        private void GameEventsOnAddObjective(Objective[] questObjectives, bool isMainQuest)
        {
            foreach (var objective in questObjectives)
            {
                var temp = Instantiate(questPanel, questRootLocation);
                
                if (isMainQuest) temp.transform.SetSiblingIndex(mainQuestPanel.transform.GetSiblingIndex() + 1);
                else sideQuestPanel.SetActive(true);
                
                temp.name = string.Format(objective.objectiveDisplayName, objective.currentValue, objective.targetComplete);
                
                var objectiveUI = temp.GetComponent<ObjectiveUI>();
                objectiveUI.NewObjective();
                objectiveUI.BlinkAnimation();
                objectiveUI.ObjectiveText(temp.name);
                objectiveUI.ShowCheckmark(objective.isCompleted);
                objective.ui = objectiveUI;
            }
        }

        #endregion
    }
}
