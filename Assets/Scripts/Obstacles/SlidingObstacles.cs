using System.Collections;
using Enums;
using UnityEngine;
using static UnityEngine.Random;

namespace Obstacles
{
    public class SlidingObstacles : ObstacleSetup
    {
        [SerializeField] private float boxArrivalTime;

        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private float interval;
        [SerializeField] private float leftSpawnPoint;
        [SerializeField] private float rightSpawnPoint;

        public override void Activate()
        {
            foreach (var m in MovingObstacles)
            {
                var isStartingFromLeft = Range(0, 2) == 0;
                var obstaclePosition = m.transform.position;

                m.transform.position = new Vector3(isStartingFromLeft ? leftSpawnPoint : rightSpawnPoint,
                    obstaclePosition.y, obstaclePosition.z);
                m.Direction = isStartingFromLeft ? Direction.LeftToRight : Direction.RightToLeft;

                var distance = Range(minDistance, maxDistance);
                m.MoveDistance = distance;
                m.Interval = boxArrivalTime;
            }

            StartCoroutine(StartSliding());
        }

        private IEnumerator StartSliding()
        {
            foreach (var m in MovingObstacles)
            {
                m.gameObject.SetActive(true);
                m.Activate();
                yield return new WaitForSeconds(interval);
            }
        }
    }
}