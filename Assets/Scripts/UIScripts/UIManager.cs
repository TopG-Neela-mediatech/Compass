using AssetKits.ParticleImage;
using UnityEngine;

namespace TMKOC.Compass
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private ParticleImage confettiPreafab;


        public void PlayConfetti()=>confettiPreafab.Play();
    }
}
