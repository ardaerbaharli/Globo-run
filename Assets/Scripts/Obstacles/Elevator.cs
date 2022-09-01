using System.Collections.Generic;
using Enums;
using NaughtyAttributes;
using UnityEngine;

namespace Obstacles
{
    public class Elevator : ObstacleSetup
    {
        [SerializeField] private bool randomizeInterval;

        [HideIf("randomizeInterval")] [SerializeField]
        private float interval;

        [ShowIf("randomizeInterval")] [SerializeField]
        private float minTime;

        [ShowIf("randomizeInterval")] [SerializeField]
        private float maxTime;

        [SerializeField] private bool randomizeDirection;

        [HideIf("randomizeInterval")] [SerializeField]
        private List<Direction> directions;

        [ShowIf("randomizeDirection")] [SerializeField]
        private float topPositionY;

        [ShowIf("randomizeDirection")] [SerializeField]
        private float bottomPositionY;

        [SerializeField] private bool randomizeMoveDistance;

        [HideIf("randomizeMoveDistance")] [SerializeField]
        private float moveDistance;

        [ShowIf("randomizeMoveDistance")] [SerializeField]
        private float minMoveDistance;

        [ShowIf("randomizeMoveDistance")] [SerializeField]
        private float maxMoveDistance;


        public override void Activate()
        {
            var i = 0;
            foreach (var m in MovingObstacles)
            {
                if (randomizeDirection)
                {
                    var dir = (Direction) Random.Range(2, 4);
                    m.Direction = dir;

                    var pos = m.transform.localPosition;
                    pos.y = dir == Direction.Downwards ? topPositionY : bottomPositionY;
                    m.transform.localPosition = pos;
                }
                else
                {
                    m.Direction = directions[i];
                    i++;
                }

                m.Interval = randomizeInterval ? Random.Range(minTime, maxTime) : interval;
                m.MoveDistance = randomizeMoveDistance ? Random.Range(minMoveDistance, maxMoveDistance) : moveDistance;

                m.Activate();
            }
        }
    }
}