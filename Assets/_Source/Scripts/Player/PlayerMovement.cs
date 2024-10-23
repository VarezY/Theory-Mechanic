using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Serialize Parameter

        [SerializeField] private float moveSpeed;
        [SerializeField] private float gravity = -9.81f;
        
        #endregion

        #region Private Parameter
        
        private CharacterController _characterController;
        private Vector2 _moveInputValue;
        private Vector3 _playerVelocity;
        private bool _isGrounded;

        #endregion

        #region Unity Function

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
        }

        private void Update()
        {
            CheckGravity();
            
            MoveCharacter();
            
            ApplyGravity();
        }

        private void LateUpdate()
        {
        }

        #endregion

        #region Private Methods

        private void MoveCharacter()
        {
            Vector3 move = transform.right * _moveInputValue.x + transform.forward * _moveInputValue.y;
            _characterController.Move(move * (moveSpeed * Time.deltaTime));
        }

        private void ApplyGravity()
        {
            _playerVelocity.y += gravity * Time.deltaTime;
            _characterController.Move(_playerVelocity * Time.deltaTime);
        }

        private void CheckGravity()
        {
            _isGrounded = _characterController.isGrounded;
            if (_isGrounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -2f;
            }
        }

        #endregion
        
        #region Player Inputs

        private void OnMove(InputValue moveValue)
        {
            _moveInputValue = moveValue.Get<Vector2>();
        }

        #endregion 
    }
}
