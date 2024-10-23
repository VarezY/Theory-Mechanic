using System;
using Managers;
using UnityEngine;
namespace Skyboxes
{
    [ExecuteAlways]
    public class LightingManager : MonoBehaviour
    {
        private enum ClockSetting { Hours12, Hours24, }

        private enum ClockAMPM { am, pm, }

        #region Serialize Parameter
        
        [SerializeField] private Light directionalLight;
        [SerializeField] private LightingPreset preset;

        [SerializeField] private ClockSetting clockSetting;
        
        [SerializeField, Range(0, 10)] private float timeRate = 1;
        [SerializeField, Range(0, 24)] private float timeOfDay;
        [SerializeField, Range(0, 12)] private float timeOfDay12;
        
        #endregion

        #region Private Parameter
        
        private bool _clockNoon;
        private ClockAMPM _clockAmpm;
        
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
            if (!preset)
                return;

            if (Application.isPlaying)
            {
                ClockCalculation();
            }
            else
            {
                UpdateLighting(timeOfDay / 24f);
            }
        }

        private void LateUpdate()
        {
        }

        #endregion

        #region Public Methods

        public void AddTime(float minutes)
        {
            float addedTime = minutes / 60;
            timeOfDay += Mathf.Repeat(addedTime, 24f);
        }

        #endregion
        
        #region Private Methods

        private void ClockCalculation()
        {
            timeOfDay += (timeRate * Time.deltaTime);
            timeOfDay %= 24; //Modulus to ensure always between 0-24
            
            switch (clockSetting)
            {
                case ClockSetting.Hours12:
                    _clockNoon = false;
                    _clockAmpm = timeOfDay > 12 ? ClockAMPM.pm : ClockAMPM.am;
                    timeOfDay12 = timeOfDay % 12;
                    break;
                case ClockSetting.Hours24:
                    _clockNoon = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            UpdateLighting(timeOfDay / 24f);
            
            GameManager.Instance.GameEvents.UpdateTimeUI(timeOfDay, _clockNoon);
        }

        private void UpdateLighting(float timePercent)
        {
            //Set ambient and fog
            RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
            RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

            //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
            if (!directionalLight)
                return;
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);

            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

        //Try to find a directional light to use if we haven't set one
        private void OnValidate()
        {
            if (directionalLight != null)
                return;

            //Search for lighting tab sun
            if (RenderSettings.sun != null)
            {
                directionalLight = RenderSettings.sun;
            }
            //Search scene for light that fits criteria (directional)
            else
            {
                Light[] lights = GameObject.FindObjectsOfType<Light>();
                foreach (var directionalLight1 in lights)
                {
                    if (directionalLight1.type != LightType.Directional)
                        continue;
                    directionalLight = directionalLight1;
                    return;
                }
            }
        }

        #endregion
    }
}