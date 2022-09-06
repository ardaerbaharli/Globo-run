using Enums;
using NaughtyAttributes;
using UnityEngine;

namespace Obstacles
{
    public class TripleMovingGate : ObstacleSetup
    {
        [SerializeField] private bool randomizeInterval;

        [HideIf("randomizeInterval")] [SerializeField]
        private float interval;

        [ShowIf("randomizeInterval")] [SerializeField]
        private float minTime;

        [ShowIf("randomizeInterval")] [SerializeField]
        private float maxTime;

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
            
            foreach (var gate in MovingObstacles)
            {
                gate.Direction = Direction.Downwards;
                gate.Interval = randomizeInterval ? Random.Range(minTime, maxTime) : interval;
                gate.MoveDistance =
                    randomizeMoveDistance ? Random.Range(minMoveDistance, maxMoveDistance) : moveDistance;

                gate.Activate();
            }
        }
    }
}