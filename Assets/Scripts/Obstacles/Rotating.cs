using Enums;
using UnityEngine;
using static Utilities.iTween;

namespace Obstacles
{
    public class Rotating : Obstacle
    {
        [Header("Settings")] public RotateDirection rotateDirection = RotateDirection.Clockwise;
        public EaseType easeType;
        public LoopType loopType;
        public Axis axis;
        public float interval = 5f;

        private int _rotateDirection;
        private string _axis;

        private void OnEnable()
        {
            if (rotateDirection == RotateDirection.Clockwise)
                _rotateDirection = 1;
            else
                _rotateDirection = -1;

            _axis = axis == Axis.X ? "x" : axis == Axis.Y ? "y" : "z";

            RotateBy(gameObject,
                Hash(_axis, _rotateDirection, "time", interval, "looptype", loopType, "easetype",
                    easeType));
        }
        
}
}