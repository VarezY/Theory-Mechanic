using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace UI
{
    public class TextPrompt : MonoBehaviour
    {
        public GameObject arrow;

        public void SelectIcon(bool value)
        {
            arrow.SetActive(value);
        }
    }
}
