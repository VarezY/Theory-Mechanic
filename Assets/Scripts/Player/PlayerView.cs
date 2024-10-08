using UnityEngine;
using UnityEngine.InputSystem;
namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        #region Serialize Parameter

        [SerializeField] private float cameraSensitivity;
        
        #endregion

        #region Private Parameter

        private Camera _camera;
        private Vector2 _cameraInputValue;
        private float _verticalRotation = 0f;

        #endregion

        #region Unity Function

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
        }

        private void Update()
        {
            CameraRotation();
        }

        private void LateUpdate()
        {
            
        }

        #endregion

        #region Public Methods

        

        #endregion
        
        #region Private Methods

        private void CameraRotation()
        {
            _verticalRotation -= _cameraInputValue.y;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);

            transform.Rotate(Vector3.up * (_cameraInputValue.x));
            _camera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        }

        #endregion
        
        #region Player Inputs

        private void OnLook(InputValue value)
        {
            _cameraInputValue = value.Get<Vector2>() * cameraSensitivity;
        }

        #endregion
    }
}
