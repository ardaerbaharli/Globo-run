using System;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        private void Awake()
        {
            var renderer = GetComponent<Renderer>();
            var num = Random.Range(0, 5);
            renderer.material = Resources.Load<Material>("Materials/" + num);
        }
      
    }
}