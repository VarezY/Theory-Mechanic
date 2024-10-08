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

        [SerializeField] private GameObject buttonPrompt;
        [SerializeField] private Transform rootLocation;
        
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

        private void CheckOutOfBounds()
        {
            
        }

        #endregion
    }
}
