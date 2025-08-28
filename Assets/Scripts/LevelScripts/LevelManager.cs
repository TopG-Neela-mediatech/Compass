using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


namespace TMKOC.Compass
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform buttonParentT;
        [SerializeField] private LevelSO levels;
        [SerializeField] private OptionManager[] optionButtons;
        private Tween pulseEffectTween;
        private int CurrentLevelIndex;


        public NeedlePoints GetCorrectAnswer() => levels.levels[CurrentLevelIndex].correctDirection;
        public void StartLevel() => GameManager.Instance.InvokeLevelStart();


        private void Awake()
        {
            ShuffleList(levels.levels);
            CurrentLevelIndex = 0;
        }
        private void Start()
        {
            GameManager.Instance.OnLevelStart += OnLevelStart;
            GameManager.Instance.OnLevelLose += OnLevelLose;
            GameManager.Instance.OnLevelWin += OnLevelWin;
            MoveButtonsOut();
        }
        public void StartNextLevel()
        {
            CurrentLevelIndex++;

            if (CurrentLevelIndex >= levels.levels.Count)
            {
#if PLAYSCHOOL_MAIN
                    EffectParticleControll.Instance.SpawnGameEndPanel();
                    GameOverEndPanel.Instance.AddTheListnerRetryGame();
                    return;
#endif
                CurrentLevelIndex = 0;
            }
            StartLevel();
        }
        private void OnLevelStart()
        {
            SetButtonData();
            MoveButtonUP();
        }
        private void OnLevelLose()
        {
            MoveButtonsOut();
        }
        private void OnLevelWin()
        {
            MoveButtonsOut();
            StartCoroutine(LoadNextLevelAfterDelay());
        }
        private IEnumerator LoadNextLevelAfterDelay()
        {
            yield return new WaitForSeconds(2f);
            StartNextLevel();
        }
        private void MoveButtonUP()
        {
            buttonParentT.DOLocalMoveY(0f, 1f).OnComplete(() =>
            {
                if (pulseEffectTween != null)
                {
                    pulseEffectTween.Play();
                    return;
                }
                pulseEffectTween = buttonParentT.DOScale(1.05f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            });
        }

        private void SetButtonData()
        {
            List<NeedlePoints> options = new List<NeedlePoints>();
            NeedlePoints correct = levels.levels[CurrentLevelIndex].correctDirection;
            options.Add(correct);
            List<NeedlePoints> allOthers = new List<NeedlePoints>();
            foreach (var level in levels.levels)
            {
                if (!allOthers.Contains(level.correctDirection) && level.correctDirection != correct)
                {
                    allOthers.Add(level.correctDirection);
                }
            }
            ShuffleList(allOthers);
            for (int i = 0; i < Mathf.Min(3, allOthers.Count); i++)
            {
                options.Add(allOthers[i]);
            }
            ShuffleList(options);
            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionButtons[i].SetOptionButton(options[i].ToString(), options[i]);
            }
        }
        private void MoveButtonsOut()
        {
            if (pulseEffectTween != null)
            {
                pulseEffectTween.Pause();
            }
            buttonParentT.DOLocalMoveY(-500f, 0f);
        }
        private void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                // Swap elements
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }
    public enum NeedlePoints
    {
        None,
        North,
        South,
        East,
        West,
        NorthWest,
        NorthEast,
        SouthWest,
        SouthEast
    }
}
