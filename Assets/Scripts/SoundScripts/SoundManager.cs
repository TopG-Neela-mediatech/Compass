using System;
using UnityEngine;

namespace TMKOC.Compass
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundSO englishSO;
        [SerializeField] private AudioSource levelAudioSource;
        [SerializeField] private string audioLocalization;      
        private SoundSO soundData;


        private void Awake()
        {
            SetLanguage();
        }
        private void SetLanguage()
        {
            audioLocalization = PlayerPrefs.GetString("PlayschoolLanguageAudio", audioLocalization);
            switch (audioLocalization)
            {
                case "English":
                    soundData = englishSO;
                    break;
                case "EnglishUS":
                    soundData = englishSO;
                    break;
                default:
                    soundData = englishSO;
                    break;
            }
        }
    }
}
