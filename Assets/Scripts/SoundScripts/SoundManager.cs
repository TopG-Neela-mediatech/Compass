using System;
using UnityEngine;

namespace TMKOC.Compass
{
    public class SoundManager : MonoBehaviour
    {
        /*[SerializeField] private SoundSO englishSO;
        [SerializeField] private SoundSO hindiSO;
        [SerializeField] private SoundSO tamilSO;
        [SerializeField] private SoundSO frenchSO;
        [SerializeField] private SoundSO malayalamSO;
        [SerializeField] private SoundSO punjabiSO;
        [SerializeField] private SoundSO marathiSO;
        [SerializeField] private SoundSO bengaliSO;
        [SerializeField] private AudioSource levelAudioSource;
        [SerializeField] private string audioLocalization;
        private SoundSO soundData;*/
        [SerializeField] private AudioMapper audioMapper;


        private void Awake()
        {
           // SetLanguage();
        }
        private void Start()
        {
            GameManager.Instance.OnLevelStart += PlayGenericQuestions;
            GameManager.Instance.OnLevelLose += PlayRetryAudio;
        }
        /* private void PlayLevelAudio(AudioClip clip)
         {
             if (clip != null)
             {
                 levelAudioSource.PlayOneShot(clip);
             }
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
                 case "French":
                     soundData = frenchSO;
                     break;
                 case "Punjabi":
                     soundData = punjabiSO;
                     break;
                 case "Malayalam":
                     soundData = malayalamSO;
                     break;
                 case "Marathi":
                     soundData = marathiSO;
                     break;
                 case "Bengali":
                     soundData = bengaliSO;
                     break;
                 default:
                     soundData = englishSO;
                     break;
             }
         }
        */
        public float PlayIntro()//for level one play intro/welcome audio
        {
           // if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
            float f = RuntimeAudioLoader.Instance.PlayRuntimeAudio(audioMapper.gameIntro);
            return f;
        }
        public void PlayOutro()//for level one play intro/welcome audio
        {
           // if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
            RuntimeAudioLoader.Instance.PlayRuntimeAudio(audioMapper.gameOutro);
        }
        public float PlayFlashCardAudio(int index)
        {
            //if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
            float f = RuntimeAudioLoader.Instance.PlayRuntimeAudio(audioMapper.directionAudio[index]);
            return f;
        }
        private void PlayGenericQuestions()
        {
           // if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
            int rand = UnityEngine.Random.Range(0, audioMapper.genericQuestionAudio.Length);
            float f = RuntimeAudioLoader.Instance.PlayRuntimeAudio(audioMapper.genericQuestionAudio[rand]);
        }
        public void PlayCorrectAudio()
        {
            /* if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
             int rand = UnityEngine.Random.Range(0, soundData.correctAudio.Length);
             PlayLevelAudio(soundData.correctAudio[rand]);*/
            RuntimeAudioLoader.Instance.PlayCorrectAudioClip();
        }
        public void PlayInCorrectAudio()
        {
            /* if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
             int rand = UnityEngine.Random.Range(0, soundData.incorrectAudio.Length);
             PlayLevelAudio(soundData.incorrectAudio[rand]);*/
            RuntimeAudioLoader.Instance.PlayIncorrectAudioClip();
        }
        private void PlayRetryAudio()
        {
           /* if (levelAudioSource.isPlaying) { levelAudioSource.Stop(); }
            int rand = UnityEngine.Random.Range(0, soundData.retryAudio.Length);
            PlayLevelAudio(soundData.retryAudio[rand]);*/
           RuntimeAudioLoader.Instance.PlayRetryAudioClip();
        }       
        private void OnDestroy()
        {
            GameManager.Instance.OnLevelStart -= PlayGenericQuestions;
            GameManager.Instance.OnLevelLose -= PlayRetryAudio;
        }
    }
}
