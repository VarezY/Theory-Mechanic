using UnityEngine;
namespace Tools
{
    public class BillboardIndicator : MonoBehaviour
    {
        private Camera _camera;
        private void Start()
        {
            _camera = Camera.main;
        }
        private void Update()
        {
            if (_camera)
                transform.rotation = Quaternion.Euler(0f, _camera.transform.rotation.eulerAngles.y, 0f);
        }
    }
}
