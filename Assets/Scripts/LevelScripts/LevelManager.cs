using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;


namespace TMKOC.Compass
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform buttonParentT;
        [SerializeField] private LevelSO levels;
        [SerializeField] private OptionManager[] optionButtons;
        private int CurrentLevelIndex;

        public NeedlePoints GetCorrectAnswer() => levels.levels[CurrentLevelIndex].correctDirection;


        private void Awake()
        {
            ShuffleList(levels.levels);
        }
        private void Start()
        {
            MoveButtonsOut();
            CurrentLevelIndex = 0;
            SetButtonData();
        }
        private void MoveButtonUP()
        {
            buttonParentT.DOLocalMoveY(100f, 1f);
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
            buttonParentT.DOLocalMoveY(-500f, 0f).OnComplete(() =>
            {
                MoveButtonUP();
            });
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
