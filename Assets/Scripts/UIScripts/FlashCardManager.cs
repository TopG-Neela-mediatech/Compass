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
        [SerializeField] private float aspectRatioThreshold = 0.5f;//for portrait
        [SerializeField] private Sprite[] directionSprite;
        [SerializeField] private Transform needleImageTransform;
        [SerializeField] private Transform compassParentT;
        [SerializeField] private Transform frameImageT;
        [SerializeField] private TextMeshProUGUI directionNametext;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;
        [SerializeField] private ParticleSystem starEffect;
        private int index;


        private void Awake()
        {
            AdjustSize();
        }
        private void Start()
        {
            index = 7;
            RotateNeedle(GetNeedlePoint(index + 1));
            flashCardImage.sprite = directionSprite[index];
            nextButton.onClick.AddListener(LoadNextSprite);
            prevButton.onClick.AddListener(LoadPrevSprite);
            DoFlashCardAnimation(false);
            DisableButtons();
        }
        private void AdjustSize()
        {
            float aspectRatio = (float)Screen.width / Screen.height;           
            if (aspectRatio > aspectRatioThreshold)//greater here because portrait
            {
                SetTabletSizing();
                return;
            }
        }
        private void SetTabletSizing()
        {
            compassParentT.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            frameImageT.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
        private void DoFlashCardAnimation(bool prev)
        {
            DisableButtons();
            float val = prev ? -1200f : 1200f;
            frameImageT.DOLocalMoveX(val, 0).OnComplete(() =>
            {
                frameImageT.DOLocalMoveX(0f, 1.5f).OnComplete(() =>
                 {
                     DOVirtual.DelayedCall(0.5f, EnableButtons);
                 }
                 );
            });
        }
        private void DisableButtons()
        {
            nextButton.enabled = false;
            prevButton.enabled = false;
        }
        private void EnableButtons()
        {
            nextButton.enabled = true;
            prevButton.enabled = true;
        }
        private NeedlePoints GetNeedlePoint(int val)
        {
            directionNametext.text = ((NeedlePoints)val).ToString();
            DoTextAnimation();
            return (NeedlePoints)val;
        }
        private void DoTextAnimation()
        {
            directionNametext.DOFade(0f, 0f).OnComplete(() =>
            {
                directionNametext.DOFade(1f, 2f);
            });
        }
        private void LoadNextSprite()
        {
            DoFlashCardAnimation(false);
            index++;
            if (index >= directionSprite.Length)
            {
                GameManager.Instance.LevelManager.StartLevel();
                flashCardParent.SetActive(false);
                return;
            }
            flashCardImage.sprite = directionSprite[index];
            starEffect.Play();
            RotateNeedle(GetNeedlePoint(index + 1));
        }
        private void LoadPrevSprite()
        {
            if (index <= 0)
            {
                index = 0;
                return;
            }
            DoFlashCardAnimation(true);
            index--;
            flashCardImage.sprite = directionSprite[index];
            RotateNeedle(GetNeedlePoint(index + 1));
            starEffect.Play();
        }
        public void RotateNeedle(NeedlePoints point)
        {
            float targetAngle = GetPositiveAngle(point);
            int fullRotations = 3;
            float currentZ = needleImageTransform.localEulerAngles.z;
            float delta = (targetAngle - currentZ + 360f) % 360f;
            float totalAngle = currentZ + delta + 360f * fullRotations;
            needleImageTransform
                .DOLocalRotate(new Vector3(0f, 0f, -totalAngle), 1f, RotateMode.FastBeyond360)
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
        /*private IEnumerator EnableButtonAfterDelay()
        {
            yield return new WaitForSeconds(3f);
            EnableButtons();
        }*/
    }
}
