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

            bool musicMuted = GP_Player.GetBool("music_muted");
            bool soundMuted = GP_Player.GetBool("sound_muted");

            if (musicMuted) buttonMusic.gameObject.GetComponent<ToggleUI>().UpdateIcon();
            if (soundMuted) buttonSound.gameObject.GetComponent<ToggleUI>().UpdateIcon();

            int level = GP_Player.GetInt("level");
            UpdateLevelNumber(level);
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
