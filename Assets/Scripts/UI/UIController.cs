using System;
using Item;
using Managers;
using MyBox;
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
        [SerializeField] private Transform rootLocation;

        [Header("Slider Status")]
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider staminaSlider;
        [SerializeField] private Slider boredomSlider;
        
        [Header("Clock")]
        [SerializeField] private TMP_Text clockText;
        
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
            GameManager.Instance.GameEvents.onUpdateTimeUI += GameEventsOnUpdateTimeUI;
        }

        private void OnEnable()
        {
        }
        
        private void OnDisable()
        {
            GameManager.Instance.GameEvents.onUpdateTimeUI -= GameEventsOnUpdateTimeUI;
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
            foreach (ItemCategorize categorized in Enum.GetValues(typeof(ItemCategorize)))
            {
                if (categorized == ItemCategorize.None || !targetItem.categorize.HasFlag(categorized))
                    continue;
                
                var tempButton = Instantiate(buttonPrompt, rootLocation);
                tempButton.name = categorized.ToString();
                tempButton.GetComponentInChildren<TMP_Text>().text = categorized.ToString();

                switch (categorized)
                {
                    case ItemCategorize.Interact1:
                        if (!string.IsNullOrEmpty(targetItem.interactName1))
                            tempButton.GetComponentInChildren<TMP_Text>().text = targetItem.interactName1;
                        
                        tempButton.GetComponent<Button>().onClick.AddListener(() => targetItem.interact1.Invoke());
                        break;
                    case ItemCategorize.Interact2:
                        if (!string.IsNullOrEmpty(targetItem.interactName2))
                            tempButton.GetComponentInChildren<TMP_Text>().text = targetItem.interactName2;

                        tempButton.GetComponent<Button>().onClick.AddListener(() => targetItem.interact2.Invoke());
                        break;
                    case ItemCategorize.World:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (SelectedButton)
                    continue;
                SelectedButton = tempButton.GetComponent<Button>();
                SelectedButton.GetComponent<TextPrompt>().SelectIcon(true);
                _indexButton = 0;
            }
        }

        public void SelectButtonPrompt(int index)
        {
            Button[] buttonList = rootLocation.GetComponentsInChildren<Button>();
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
            foreach (Transform child in rootLocation)
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

        #endregion
    }
}
