using System.Collections.Generic;
using Enums;
using NaughtyAttributes;
using UnityEngine;

namespace Obstacles
{
    public class MovingGate : ObstacleSetup
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
        private float leftSpawnX;

        [ShowIf("randomizeDirection")] [SerializeField]
        private float rightSpawnX;

        [SerializeField] private bool randomizeMoveDistance;

        [HideIf("randomizeMoveDistance")] [SerializeField]
        private float moveDistance;

        [ShowIf("randomizeMoveDistance")] [SerializeField]
        private float minMoveDistance;

        [ShowIf("randomizeMoveDistance")] [SerializeField]
        private float maxMoveDistance;


        public override void Activate()
        {
            if (_isActivated) return;
            _isActivated = true;
            
            var i = 0;
            foreach (var m in MovingObstacles)
            {
                if (randomizeDirection)
                {
                    var dir = (Direction) Random.Range(0, 2);
                    m.Direction = dir;

                    var pos = m.transform.localPosition;
                    pos.x = dir == Direction.RightToLeft ? rightSpawnX : leftSpawnX;
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