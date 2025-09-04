using System;
using UnityEngine;

namespace TMKOC.Compass
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundSO englishSO;
        [SerializeField] private SoundSO hindiSO;
        [SerializeField] private SoundSO tamilSO;
        [SerializeField] private AudioSource levelAudioSource;
        [SerializeField] private string audioLocalization;
        private SoundSO soundData;


        private void Awake()
        {
            SetLanguage();
        }
        private void Start()
        {
            GameManager.Instance.OnLevelStart += PlayGenericQuestions;
            GameManager.Instance.OnLevelLose += PlayRetryAudio;
        }
        private void PlayLevelAudio(AudioClip clip)
        {
            if (clip != null)
            {
                levelAudioSource.PlayOneShot(clip);
            }
        }
        public float PlayIntro()//for level one play intro/welcome audio
        {
            if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
            PlayLevelAudio(soundData.gameIntro);
            return soundData.gameIntro.length;
        }
        public void PlayOutro()//for level one play intro/welcome audio
        {
            if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
            PlayLevelAudio(soundData.gameOutro);
        }
        public void PlayFlashCardAudio(int index)
        {
            if(levelAudioSource.isPlaying) {levelAudioSource.Stop(); }
            PlayLevelAudio(soundData.directionAudio[index]);
        }
        private void PlayGenericQuestions()
        {
            if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
            int rand = UnityEngine.Random.Range(0, soundData.genericQuestionAudio.Length);
            PlayLevelAudio(soundData.genericQuestionAudio[rand]);
        }
        public void PlayCorrectAudio()
        {
            if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
            int rand = UnityEngine.Random.Range(0, soundData.correctAudio.Length);
            PlayLevelAudio(soundData.correctAudio[rand]);
        }
        public void PlayInCorrectAudio()
        {
            if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
            int rand = UnityEngine.Random.Range(0, soundData.incorrectAudio.Length);
            PlayLevelAudio(soundData.incorrectAudio[rand]);
        }
        private void PlayRetryAudio()
        {
            if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
            int rand = UnityEngine.Random.Range(0, soundData.retryAudio.Length);
            PlayLevelAudio(soundData.retryAudio[rand]);
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
                case "Hindi":
                    soundData = hindiSO;
                    break;
                case "Tamil":
                    soundData = tamilSO;
                    break;
                default:
                    soundData = englishSO;
                    break;
            }
        }
        private void OnDestroy()
        {
            GameManager.Instance.OnLevelStart -= PlayGenericQuestions;
            GameManager.Instance.OnLevelLose -= PlayRetryAudio;
        }
    }
}
