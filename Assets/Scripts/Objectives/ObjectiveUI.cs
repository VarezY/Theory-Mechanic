using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Objectives
{
    public class ObjectiveUI : MonoBehaviour
    {
        #region Serialize Parameter

        [SerializeField] private Image panelBackground;
        [SerializeField] private Image checkmark;

        [SerializeField] private Color newObjectiveColor;
        [SerializeField] private Color objectiveCompleatedColor;

        [SerializeField] private TMP_Text objectiveText;
        
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
        
        private void OnEnable()
        {
        }
        
        private void OnDisable()
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

        public void ShowCheckmark(bool show)
        {
            checkmark.gameObject.SetActive(show);
        }

        public void NewObjective()
        {
            panelBackground.color = newObjectiveColor;
        }

        public void ObjectiveCompleated()
        {
            panelBackground.color = objectiveCompleatedColor;
        }

        public void ObjectiveText(string text)
        {
            objectiveText.text = text;
        }
        
        public void BlinkAnimation()
        {
            panelBackground.DOFade(1, 0.35f).SetEase(Ease.Linear).SetLoops(10, LoopType.Yoyo);
        }
        
        #endregion
        
        #region Private Methods

        #endregion
    }
}