using UnityEngine;

namespace TMKOC.Compass
{
    [CreateAssetMenu(fileName = "SoundSO", menuName = "Scriptable Objects/SoundSO")]
    public class SoundSO : ScriptableObject
    {
        public AudioClip gameIntro;
        public AudioClip gameOutro;
        public AudioClip[] directionAudio;
        public AudioClip[] genericQuestionAudio;
        public AudioClip[] incorrectAudio;
        public AudioClip[] correctAudio;
        public AudioClip[] retryAudio;
    }
}
