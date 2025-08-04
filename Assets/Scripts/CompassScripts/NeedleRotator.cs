using DG.Tweening;
using UnityEngine;


namespace TMKOC.Compass
{
    public class NeedleRotator : MonoBehaviour
    {
        [SerializeField] private Transform needleTransform;


        public void RotateNeedle(NeedlePoints point)
        {
            
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
