using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace X2SLIME3D
{
    public class ToggleUI : MonoBehaviour
    {
        public Sprite spriteOn;
        public Sprite spriteOff;

        Image buttonImage;

        bool isOn = true;

        void Awake()
        {
            buttonImage = GetComponent<Image>();
        }

        public void UpdateIcon()
        {
            isOn = !isOn;
            buttonImage.sprite = isOn ? spriteOn : spriteOff;
        }
    }
}
