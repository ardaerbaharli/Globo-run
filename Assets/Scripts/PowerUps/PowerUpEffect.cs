using NaughtyAttributes;
using UnityEngine;

namespace PowerUps
{
    public class PowerUpEffect : ScriptableObject
    {
        public float duration;
        public bool changeMaterial;
        public bool changeColor;
        [ShowIf("changeMaterial")] public Material tileMaterial;
        [ShowIf("changeColor")] public Color tileColor;
    }
}