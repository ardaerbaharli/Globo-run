using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;
using Utilities;

namespace Obstacles
{
    public class ObstacleSetup : MonoBehaviour
    {
        [SerializeField] private float height;

        [SerializeField] protected List<Obstacle> obstacles;
        private List<Collider> _colliders;
        public PooledObject pooledObject;
        public PooledObject plane;

        private void OnEnable()
        {
            // Find all the child objects with Obstacle tag
            var s = transform.GetChildObjectsWithTag("Obstacle");
            obstacles = s.Select(x => x.GetComponent<Obstacle>()).ToList();
            _colliders = obstacles.Select(x => x.GetComponent<Collider>()).ToList();
            _colliders.ForEach(x => x.enabled = true);
        }

        public void TakeBackToPool()
        {
            obstacles.ForEach(x => x.ResetPosition());
            ObjectPool.instance.TakeBack(plane);
            ObjectPool.instance.TakeBack(pooledObject);
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = new Vector3(pos.x, height, pos.z);
            obstacles.ForEach(x => x.SetStartPosition());
        }

        public void DeactivateColliders()
        {
            _colliders.ForEach(x => x.enabled = false);
        }

        public virtual void Activate()
        {
        }
    }
}