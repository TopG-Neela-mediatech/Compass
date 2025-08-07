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
        private NeedlePoints value;


        private void Start()
        {
            optionButton.onClick.AddListener(CheckIfCorrect);
        }
        public void SetOptionButton(string optionName,NeedlePoints p)
        {
            optionText.text = optionName;
            value = p;
        }
        private void CheckIfCorrect()
        {
            if (value == GameManager.Instance.LevelManager.GetCorrectAnswer())
            {
                Debug.Log("Correct");
                optionImage.color = Color.green;
            }
            else
            {
                Debug.Log("Incorrect");
                optionImage.color = Color.red;
            }
        }
    }
}
