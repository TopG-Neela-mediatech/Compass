using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace TMKOC.Compass
{
    public class FlashCardManager : MonoBehaviour
    {
        [SerializeField] private GameObject flashCardParent;
        [SerializeField] private Image flashCardImage;
        [SerializeField] private Sprite[] directionSprite;
        [SerializeField] private Transform needleImageTransform;
        [SerializeField] private TextMeshProUGUI directionNametext;
        [SerializeField] private Button nextButton;
        private int index;


        private void Start()
        {
            index = 0;
            RotateNeedle(GetNeedlePoint(index + 1));
            flashCardImage.sprite = directionSprite[index];
            nextButton.onClick.AddListener(LoadNextSprite);
        }
        private NeedlePoints GetNeedlePoint(int val)
        {
            directionNametext.text = ((NeedlePoints)val).ToString();
            return (NeedlePoints)val;
        }
        private void LoadNextSprite()
        {
            nextButton.enabled = false;
            index++;
            if (index >= directionSprite.Length)
            {
                GameManager.Instance.LevelManager.StartLevel();
                flashCardParent.SetActive(false);
                return;
            }
            StartCoroutine(EnableNextButtonAfterDelay());
            flashCardImage.sprite = directionSprite[index];
            RotateNeedle(GetNeedlePoint(index + 1));
        }
        private IEnumerator EnableNextButtonAfterDelay()
        {
            yield return new WaitForSeconds(3f);
            nextButton.enabled = true;
        }
        public void RotateNeedle(NeedlePoints point)
        {
            float targetAngle = GetPositiveAngle(point);
            int fullRotations = 3;
            float currentZ = needleImageTransform.localEulerAngles.z;
            float delta = (targetAngle - currentZ + 360f) % 360f;
            float totalAngle = currentZ + delta + 360f * fullRotations;
            needleImageTransform
                .DOLocalRotate(new Vector3(0f, 0f, -totalAngle), 2f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuad);
        }
        private float GetPositiveAngle(NeedlePoints point)
        {
            switch (point)
            {
                case NeedlePoints.North: return 0f;
                case NeedlePoints.South: return 180f;
                case NeedlePoints.East: return 90f;
                case NeedlePoints.West: return 270f;
                case NeedlePoints.NorthWest: return 315f;
                case NeedlePoints.NorthEast: return 45f;
                case NeedlePoints.SouthWest: return 225f;
                case NeedlePoints.SouthEast: return 135f;
                default: return 0f;
            }
        }
    }
}
