using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TMKOC.Compass
{
    public class CompassShaker : MonoBehaviour
    {
        [SerializeField] private Transform compassT;
        [SerializeField] private Button compassShakerButton;


        private void Start()
        {
            compassShakerButton.onClick.AddListener(ShakeCompass);
        }
        private void EnableCompassShakerButton() => compassShakerButton.enabled = true;
        private void DisableCompassShakerButton() => compassShakerButton.enabled = false;


        private void ShakeCompass()
        {
            DisableCompassShakerButton();
            compassT.DOShakePosition(2f, 0.5f).OnComplete(() =>
            {
                EnableCompassShakerButton();
            });
        }
    }
}
