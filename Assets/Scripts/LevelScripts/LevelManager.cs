using DG.Tweening;
using UnityEngine;

namespace TMKOC.Compass
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform buttonParentT;


        private void Start()
        {
            MoveButtonsOut();
        }
        private void MoveButtonsOut()
        {
            buttonParentT.DOLocalMoveY(-500f, 0f);
        }
    }
}
