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
            ShuffleLevels(levels.levels);
        }
        private void Start()
        {
            MoveButtonsOut();    
            CurrentLevelIndex = 0;
        }      
        private void MoveButtonsOut()
        {
            buttonParentT.DOLocalMoveY(-500f, 0f);
        }
        private void ShuffleLevels<T>(List<T> list)
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
