using System;
using Item;
using Managers;
using MyBox;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {
        #region Serialize Parameter

        [SerializeField] private LayerMask interactLayer;
        [SerializeField] private float interactDistance;

        #endregion

        #region Private Parameter

        private GameManager _manager;
        private Camera _camera;
        private bool _hasItem;
        private int _indexButton;

        #endregion

        #region Unity Function

        private void Awake()
        {
        }

        private void Start()
        {
            _manager = FindObjectOfType<GameManager>();
            _camera = Camera.main;
        }

        private void Update()
        {
            RayCastInteractable();
        }

        private void LateUpdate()
        {
        }

        #endregion

        #region Public Methods

        

        #endregion
        
        #region Private Methods

        private void RayCastInteractable()
        {
            if (!_camera)
                return;
            var ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out var hitData, interactDistance, interactLayer, QueryTriggerInteraction.Ignore))
            {
                // The Ray hit something less than 10 Units away,
                // It was on a certain Layer
                // But it wasn't a Trigger Collider

                if (_hasItem)
                    return;
                var itemController = hitData.transform.GetComponent<ItemController>();
                _manager.UIController.GenerateButtonPrompt(itemController);
                _hasItem = true;

            }
            else
            {
                _hasItem = false;
                _manager.UIController.ClearButtonPrompt();
            }
        }

        #endregion
        
        #region Player Inputs

        private void OnInteract()
        {
            if (!GameManager.Instance.UIController.SelectedButton)
                return;
            _manager.UIController.SelectedButton.onClick.Invoke();
            _manager.AudioController.PlayAudioInteract();
        }
        
        private void OnSelectInteract(InputValue scrollValue)
        {
            var a = scrollValue.Get<Vector2>();

            switch (a.y)
            {
                case 0:
                    return;
                case > 0:
                    _indexButton = 1;
                    break;
                case < 0:
                    _indexButton = -1;
                    break;
            }

            _manager.UIController.SelectButtonPrompt(_indexButton);
        }

        #endregion
    }
}
