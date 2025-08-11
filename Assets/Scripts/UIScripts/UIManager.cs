using AssetKits.ParticleImage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TMKOC.Compass
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private ParticleImage confettiPreafab;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button playSchoolBackbutton;


        public void PlayConfetti() => confettiPreafab.Play();


        private void Start()
        {
            GameManager.Instance.OnLevelWin += EnableWinPanel;
            GameManager.Instance.OnLevelLose += EnableLosePanel;
            nextButton.onClick.AddListener(LoadNextLevel);
            restartButton.onClick.AddListener(RestartLevel);
            playSchoolBackbutton.onClick.AddListener(() => SceneManager.LoadScene(TMKOCPlaySchoolConstants.TMKOCPlayMainMenu));
        }
        private void EnableWinPanel()
        {
            winPanel.SetActive(true);
            nextButton.enabled = true;
        }
        private void EnableLosePanel()
        {
            restartButton.enabled = true;
            losePanel.SetActive(true);
        }
        private void LoadNextLevel()
        {
            nextButton.enabled = false;
            DisablePanels();
            GameManager.Instance.LevelManager.StartNextLevel();
        }
        private void RestartLevel()
        {
            restartButton.enabled = false;
            DisablePanels();
            GameManager.Instance.LevelManager.StartLevel();
        }
        private void DisablePanels()
        {
            winPanel.SetActive(false);
            losePanel.SetActive(false);
        }
        private void OnDestroy()
        {
            GameManager.Instance.OnLevelWin -= EnableWinPanel;
            GameManager.Instance.OnLevelLose -= EnableLosePanel;
        }
    }
}
