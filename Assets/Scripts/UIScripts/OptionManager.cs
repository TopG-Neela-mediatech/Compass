using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TMKOC.Compass
{
    public class OptionManager : MonoBehaviour
    {
        [SerializeField] private Button optionButton;
        [SerializeField] private TextMeshProUGUI optionText;
        [SerializeField] private Image optionImage;
        private static event Action OnButtonPressed;      
        private NeedlePoints value;
        private Color startColor;


        private void DisableButton() => optionButton.enabled = false;
        private void EnableButton() => optionButton.enabled = true;


        private void Start()
        {
            startColor = optionImage.color;
            optionButton.onClick.AddListener(CheckIfCorrect);
            GameManager.Instance.OnLevelStart += OnLevelStart;
            GameManager.Instance.LivesManager.OnLivesReducedAnimationOver += OnLivesAnimationOver;
            OnButtonPressed += OnButtonClicked;
        }
        private void OnLivesAnimationOver()
        {
            EnableButton();
            optionImage.color = startColor;
        }
        private void OnLevelStart()
        {
            EnableButton();
            ResetButton();
        }
        private void ResetButton()
        {
            optionImage.color = startColor;
        }
        private void OnButtonClicked()
        {
            DisableButton();
        }
        public void SetOptionButton(string optionName, NeedlePoints p)
        {
            optionText.text = optionName;
            value = p;
        }
        private IEnumerator InvokeLevelWinAfterDelay()
        {
            yield return new WaitForSeconds(3f);
            GameManager.Instance.InvokeLevelWin();
        }
        private void CheckIfCorrect()
        {
            OnButtonPressed?.Invoke();
            if (value == GameManager.Instance.LevelManager.GetCorrectAnswer())
            {
                optionImage.color = Color.green;
                StartCoroutine(InvokeLevelWinAfterDelay());
                GameManager.Instance.UIManager.PlayConfetti();
            }
            else
            {
                GameManager.Instance.CompassShaker.ShakeCompass();
                optionImage.color = Color.red;
                GameManager.Instance.LivesManager.ReduceLives();
            }
        }
        private void OnDestroy()
        {
            GameManager.Instance.OnLevelStart -= OnLevelStart;
            OnButtonPressed -= OnButtonClicked;
            GameManager.Instance.LivesManager.OnLivesReducedAnimationOver -= OnLivesAnimationOver;
        }
    }
}
