using UnityEngine;
namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        #region Serialize Parameter

        [SerializeField] private AudioSource playerSource;
        [Header("Interact")]
        [SerializeField] private AudioClip changeButton;
        [SerializeField] private AudioClip interactButton;

        [Header("Objective")]
        [SerializeField] private AudioClip objectiveNew;
        [SerializeField] private AudioClip objectiveCompleted;
        
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

        public void PlayAudioNewObjective()
        {
            playerSource.PlayOneShot(objectiveNew);
        }

        public void PlayAudioCompleteObjective()
        {
            playerSource.PlayOneShot(objectiveCompleted);
        }

        #endregion
        
        #region Private Methods

        

        #endregion

        #region Player Inputs

        #endregion
    }
}
