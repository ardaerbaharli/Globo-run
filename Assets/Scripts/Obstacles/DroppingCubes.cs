using System.Collections;
using Enums;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace Obstacles
{
    public class DroppingCubes : ObstacleSetup
    {
        [SerializeField] private float moveDistance;
        [SerializeField] private float boxArrivalTime;
        [SerializeField] private bool randomizeFallPositions;

        [ShowIf("randomizeFallPositions")] [SerializeField]
        private float minFallPositionX;

        [ShowIf("randomizeFallPositions")] [SerializeField]
        private float maxFallPositionX;

        [SerializeField] private float interval;


        public override void Activate()
        {
            if (_isActivated) return;
            _isActivated = true;
            
            StartCoroutine(StartDropping());
        }

        private IEnumerator StartDropping()
        {
            foreach (var m in MovingObstacles)
            {
                if (randomizeFallPositions)
                {
                    var cPos = m.transform.localPosition;
                    cPos.x = Random.Range(minFallPositionX, maxFallPositionX);
                    m.transform.localPosition = cPos;
                }

                m.MoveDistance = moveDistance;
                m.Interval = boxArrivalTime;
                m.Direction = Direction.Downwards;

                m.gameObject.SetActive(true);
                m.Activate();
                yield return new WaitForSeconds(interval);
            }
        }

        public override void TakeBackToPool()
        {
            foreach (var x in MovingObstacles)
            {
                x.ResetPosition();
                x.gameObject.SetActive(false);
            }

            ObjectPool.Instance.TakeBack(pooledObject);
        }
    }
}