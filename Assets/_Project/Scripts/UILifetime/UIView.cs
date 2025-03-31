using System.Collections;
using System.Collections.Generic;
using GamePush;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace X2SLIME3D
{
    public class UIView : MonoBehaviour
    {
        public Button buttonMusic;
        public Button buttonSound;
        public Button buttonRestart;
        public TextMeshProUGUI levelNumber;
        public TextMeshProUGUI youWinText;

        Language language;

        async void Start()
        {
            await GP_Init.Ready;
            language = GP_Language.Current();
            if(language == Language.Russian) UpdateLevelNumber(1);
        }

        public void UpdateLevelNumber(int number)
        {
            if(language == Language.Russian)
                levelNumber.text = $"УРОВЕНЬ {number}/50";
            else
                levelNumber.text = $"LEVEL {number}/50";
        }

        public void ShowYouWin()
        {
            if(language == Language.Russian) youWinText.text = "ВЫ ПОБЕДИЛИ!";
            youWinText.enabled = true;
        }
    }
}
