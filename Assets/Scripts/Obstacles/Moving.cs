using System;
using Enums;
using UnityEngine;
using Utilities;
using static Utilities.iTween;

namespace Obstacles
{
    public class Moving : Obstacle
    {
        [Header("Settings")] public float moveDistance = 650;
        public float interval = 2f;
        public Direction direction;
        public Axis moveAxis;
        public EaseType easeType;
        public LoopType loopType;

        private float _moveDistance;
        private string _moveAxis;

        private void OnEnable()
        {
            if (direction == Direction.LeftToRight || direction == Direction.Upwards)
                _moveDistance = Mathf.Abs(moveDistance);
            else
                _moveDistance = -Mathf.Abs(moveDistance);

            _moveAxis = moveAxis == Axis.X ? "x" : moveAxis == Axis.Y ? "y" : "z";

            MoveBy(gameObject,
                Hash(_moveAxis, _moveDistance, "easeType", easeType, "loopType", loopType, "time",
                    interval));
        }
        
        private Vector3 startPosition;

        public override void SetStartPosition()
        {
            startPosition = transform.localPosition;
        }
        
        public override void ResetPosition()
        {
            transform.localPosition = startPosition;
        }
    }
}