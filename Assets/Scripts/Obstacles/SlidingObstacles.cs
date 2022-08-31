using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using static UnityEngine.Random;

namespace Obstacles
{
    public class SlidingObstacles : ObstacleSetup
    {
        // [SerializeField] private List<Moving> obstacles;
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private float interval;
        [SerializeField] private float leftSpawnPoint;
        [SerializeField] private float rightSpawnPoint;

        public override void Activate()
        {
            foreach (var o in obstacles)
            {
                var obstacle = (Moving) o;
                var isStartingFromLeft = Range(0, 2) == 0;
                var obstaclePosition = obstacle.transform.position;
                obstacle.transform.position = new Vector3(isStartingFromLeft ? leftSpawnPoint : rightSpawnPoint,
                    obstaclePosition.y, obstaclePosition.z);
                obstacle.direction = isStartingFromLeft ? Direction.LeftToRight : Direction.RightToLeft;
                var distance = Range(minDistance, maxDistance);
                obstacle.moveDistance = distance;
            }

            StartCoroutine(StartSliding());
        }

        private IEnumerator StartSliding()
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.gameObject.SetActive(true);
                yield return new WaitForSeconds(interval);
            }
        }

    }
}