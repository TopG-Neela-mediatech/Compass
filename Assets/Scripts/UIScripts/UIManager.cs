using AssetKits.ParticleImage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TMKOC.Compass
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private ParticleImage confettiPreafab;           
        [SerializeField] private Button playSchoolBackbutton;


        public void PlayConfetti() => confettiPreafab.Play();


        private void Start()
        {
            PlayschoolCommon.Instance.SpawnplayschoolWinLosePanel();         
            GameManager.Instance.OnLevelLose += EnableLosePanel;         
            playSchoolBackbutton.onClick.AddListener(() => SceneManager.LoadScene(TMKOCPlaySchoolConstants.TMKOCPlayMainMenu));
        }
       
        private void EnableLosePanel()
        {
            WinLosePanelScript.Instance.ShowRetryPopUp(RestartLevel);
        }       
        private void RestartLevel()
        {           
            GameManager.Instance.LevelManager.StartLevel();
        }       
        private void OnDestroy()
        {           
            GameManager.Instance.OnLevelLose -= EnableLosePanel;
        }
    }
}
