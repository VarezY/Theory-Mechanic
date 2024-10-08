using DG.Tweening;
using Managers;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
namespace Item
{
    public class ItemController : MonoBehaviour
    {
        #region Serialize Parameter

        public ItemCategorize categorize;
        [Separator]
        [ConditionalField(nameof(categorize), false, ItemCategorize.Interact1), SerializeField] public string interactName1;
        [ConditionalField(nameof(categorize), false, ItemCategorize.Interact1), SerializeField] public UnityEvent interact1;
        [Separator]
        [ConditionalField(nameof(categorize), false, ItemCategorize.Interact2), SerializeField] public string interactName2;
        [ConditionalField(nameof(categorize), false, ItemCategorize.Interact2), SerializeField] public UnityEvent interact2;
        
        #endregion

        #region Private Parameter

        private GameManager _gameManager;
        private Camera _camera;
        private float _holdingDistance = 2f;
        private float _animationDuration = .15f;
        private bool _isHoldingItem;
        
        #endregion

        #region Unity Function

        private void Awake()
        {
        }

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_isHoldingItem)
            {
                HoldingItem();
            }
        }

        private void LateUpdate()
        {
        }

        #endregion

        #region Public Methods

        public void HoldItem()
        {
            if (!_isHoldingItem)
            {
                _isHoldingItem = true;
                _gameManager.UIController.SelectedButton.GetComponentInChildren<TMP_Text>().text = "Place Item";
            }
            else
            {
                _isHoldingItem = false;
                _gameManager.UIController.SelectedButton.GetComponentInChildren<TMP_Text>().text = "Hold Item";
            }
        }

        #endregion
        
        #region Private Methods

        private void HoldingItem()
        {
            // Calculate position in front of camera
            Vector3 hoverPosition = _camera.transform.position + _camera.transform.forward * _holdingDistance;

            // Move object to hover position
            transform.DOMove(hoverPosition, _animationDuration).SetEase(Ease.OutQuad);

            // Rotate object to face camera
            transform.DORotate(_camera.transform.rotation.eulerAngles, _animationDuration);
        }

        #endregion
        
    }
}
