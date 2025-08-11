using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TMKOC.Compass
{
    public class CompassShaker : MonoBehaviour
    {
        [SerializeField] private Transform compassT;
        [SerializeField] private Button compassShakerButton;
        private Tween tween;


        private void Start()
        {
            compassShakerButton.onClick.AddListener(ShakeCompass);
        }
        private void EnableCompassShakerButton() => compassShakerButton.enabled = true;
        private void DisableCompassShakerButton() => compassShakerButton.enabled = false;


        public void ShakeCompass()
        {
            if (tween != null)
            {
                tween.Kill();
            }
            DisableCompassShakerButton();
            tween = compassT.DOShakePosition(1.5f, 0.5f, 5, 45f).OnComplete(() =>
            {
                EnableCompassShakerButton();
            });
        }
    }
}
