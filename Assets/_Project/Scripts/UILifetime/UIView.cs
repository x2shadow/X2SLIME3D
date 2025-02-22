using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace X2SLIME3D
{
    public class UIView : MonoBehaviour
    {
        public Button buttonHello;
        public Button buttonRestart;
        public TextMeshProUGUI levelNumber;

        public void UpdateLevelNumber(int number)
        {
            levelNumber.text = $"LEVEL {number}/50";
        }
    }
}
