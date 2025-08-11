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


        private void DisableButton() => optionButton.enabled = false;
        private void EnableButton() => optionButton.enabled = true;


        private void Start()
        {
            optionButton.onClick.AddListener(CheckIfCorrect);
            GameManager.Instance.OnLevelStart += OnLevelStart;
            OnButtonPressed += OnButtonClicked;
        }
        private void OnLevelStart()
        {
            EnableButton();
            ResetButton();
        }
        private void ResetButton()
        {
            optionImage.color = Color.white;
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
        private IEnumerator InvokeLevelLoseAfterDelay()
        {
            yield return new WaitForSeconds(3f);
            GameManager.Instance.InvokeLevelLose();
        }
        private void CheckIfCorrect()
        {
            OnButtonPressed?.Invoke();
            if (value == GameManager.Instance.LevelManager.GetCorrectAnswer())
            {
                Debug.Log("Correct");
                optionImage.color = Color.green;
                StartCoroutine(InvokeLevelWinAfterDelay());
                GameManager.Instance.UIManager.PlayConfetti();
            }
            else
            {
                Debug.Log("Incorrect");
                optionImage.color = Color.red;
                StartCoroutine(InvokeLevelLoseAfterDelay());
            }
        }
        private void OnDestroy()
        {
            GameManager.Instance.OnLevelStart -= OnLevelStart;
            OnButtonPressed -= OnButtonClicked;
        }
    }
}
