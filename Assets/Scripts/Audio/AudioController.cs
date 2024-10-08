using UnityEngine;
namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        #region Serialize Parameter

        [SerializeField] private AudioSource playerSource;
        [SerializeField] private AudioClip changeButton;
        [SerializeField] private AudioClip interactButton;
        
        #endregion

        #region Private Parameter
        
        
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

        public void PlayAudioSelect()
        {
            playerSource.PlayOneShot(changeButton);
        }

        public void PlayAudioInteract()
        {
            playerSource.PlayOneShot(interactButton);
        }

        #endregion
        
        #region Private Methods

        

        #endregion

        #region Player Inputs

        #endregion
    }
}
