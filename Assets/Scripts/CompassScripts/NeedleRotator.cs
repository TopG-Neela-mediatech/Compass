using DG.Tweening;
using UnityEngine;


namespace TMKOC.Compass
{
    public class NeedleRotator : MonoBehaviour
    {
        [SerializeField] private Transform needleTransform;

        private void Start()
        {
            RotateNeedle(NeedlePoints.NorthWest);
        }
        public void RotateNeedle(NeedlePoints point)
        {
            float targetAngle = GetPositiveAngle(point);
            int fullRotations = 3;
            float currentZ = needleTransform.localEulerAngles.z;
            float delta = (targetAngle - currentZ + 360f) % 360f;
            float totalAngle = currentZ + delta + 360f * fullRotations;
            needleTransform
                .DOLocalRotate(new Vector3(0f, 0f, -totalAngle), 2f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuad);
        }
        private float GetPositiveAngle(NeedlePoints point)
        {
            switch (point)
            {
                case NeedlePoints.North: return 0f;
                case NeedlePoints.South: return 180f;
                case NeedlePoints.East: return 90f;
                case NeedlePoints.West: return 270f;
                case NeedlePoints.NorthWest: return 315f;
                case NeedlePoints.NorthEast: return 45f;
                case NeedlePoints.SouthWest: return 225f;
                case NeedlePoints.SouthEast: return 135f;
                default: return 0f;
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
}
