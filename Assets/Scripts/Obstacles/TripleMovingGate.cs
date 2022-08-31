using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Obstacles
{
    public class TripleMovingGate : ObstacleSetup
    {
        // [SerializeField] private List<Moving> gates;
        [SerializeField] private float minDuration;
        [SerializeField] private float maxDuration;

        public override void Activate()
        {
            obstacles.Shuffle();

            var interval = (maxDuration - minDuration) / (obstacles.Count - 1);
            for (var i = 0; i < obstacles.Count; i++)
            {
                var o = (Moving) obstacles[i];
                o.interval = minDuration + interval * i;
            }

            foreach (var gate in obstacles)
            {
                gate.gameObject.SetActive(true);
            }
        }
    }
}