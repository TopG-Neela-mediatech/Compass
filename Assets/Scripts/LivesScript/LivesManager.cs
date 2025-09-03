using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;


namespace TMKOC.Compass
{
    public class LivesManager : MonoBehaviour
    {
        [SerializeField] private Transform heartImageT;
        [SerializeField] private TextMeshProUGUI livestext;
        public event Action OnLivesReducedAnimationOver;
        private int lives;


        private void Start()
        {
            GameManager.Instance.OnLevelStart += OnLevelStart;
        }
        private void ShakeLives()
        {
            heartImageT.DOShakePosition(1f, 20, 10).OnComplete(() =>
            {
                if (lives == 0)
                {
                    StartCoroutine(InvokeLevelLoseAfterDelay());
                    return;
                }
                else
                {                   
                    OnLivesReducedAnimationOver?.Invoke();
                    return;
                }
            });
        }
        public void ReduceLives()
        {
            lives--;           
            livestext.text = "x" + lives;
            ShakeLives();
        }
        private IEnumerator InvokeLevelLoseAfterDelay()
        {
            yield return new WaitForSeconds(2f);
            GameManager.Instance.InvokeLevelLose();
        }
        private void OnLevelStart()
        {
            lives = 3;
            livestext.text = "x" + lives;
        }
        private void OnDestroy()
        {
            GameManager.Instance.OnLevelStart -= OnLevelStart;
        }
    }
}
