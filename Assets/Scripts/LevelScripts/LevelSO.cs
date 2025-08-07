using System.Collections.Generic;
using UnityEngine;

namespace TMKOC.Compass
{
    [CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Objects/LevelSO")]
    public class LevelSO : ScriptableObject
    {
        public List<LevelData> levels;
    }
    [System.Serializable]
    public class LevelData
    {
        public NeedlePoints correctDirection;

    }
}
