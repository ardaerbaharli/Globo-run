using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class DroppingCubes : ObstacleSetup
    {
        [SerializeField] private float interval;

        [SerializeField] private List<Moving> cubes;

        public override void Activate()
        {
            StartCoroutine(StartDropping());
        }

        private IEnumerator StartDropping()
        {
            foreach (var cube in cubes)
            {
                cube.gameObject.SetActive(true);
                yield return new WaitForSeconds(interval);
            }
        }
    }
}