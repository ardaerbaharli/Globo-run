using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obstacles
{
    public class ObstacleSetup : MonoBehaviour
    {
        [SerializeField] private bool hasJumper;

        [ShowIf("hasJumper")] [SerializeField] private bool randomizeJumper;

        [ShowIf("randomizeJumper")] [SerializeField]
        private bool betweenValues;

        [ShowIf("betweenValues")] [SerializeField]
        private List<float> xValues;

        [ShowIf("randomizeJumper")] [SerializeField]
        private bool withinRange;

        [ShowIf("withinRange")] [SerializeField]
        private float leftBorderX;

        [ShowIf("withinRange")] [SerializeField]
        private float rightBorderX;


        [SerializeField] protected float height;
        protected List<Obstacle> Obstacles;
        protected List<Moving> MovingObstacles;
        private List<Collider> _colliders;
        public PooledObject pooledObject;
        public PooledObject plane;

        private GameObject jumper;

        private void Awake()
        {
            if (hasJumper)
                jumper = transform.GetChildObjectWithTag("Jumper");

            var obstacleObjects = transform.GetChildObjectsWithTag("Obstacle");
            Obstacles = obstacleObjects.Select(x => x.GetComponent<Obstacle>()).ToList();

            var movingObjects = transform.GetChildObjectsWithTag("Moving");
            MovingObstacles = movingObjects.Select(x => x.GetComponent<Moving>()).ToList();

            _colliders = new List<Collider>();
            _colliders.AddRange(Obstacles.Select(x => x.GetComponent<Collider>()).ToList());
            _colliders.AddRange(MovingObstacles.Select(x => x.GetComponent<Collider>()).ToList());
        }

        private void OnEnable()
        {
            if (hasJumper && randomizeJumper) SetJumperPosition();
            _colliders.ForEach(x => x.enabled = true);
        }

        private void SetJumperPosition()
        {
            var jPos = jumper.transform.position;

            if (withinRange)
                jPos.x = Random.Range(leftBorderX, rightBorderX);
            else if (betweenValues)
                jPos.x = xValues.RandomElement();

            jumper.transform.position = jPos;
        }

        public virtual void TakeBackToPool()
        {
            foreach (var x in MovingObstacles)
                x.ResetPosition();

            ObjectPool.instance.TakeBack(plane);
            ObjectPool.instance.TakeBack(pooledObject);
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = new Vector3(pos.x, height, pos.z);
            MovingObstacles.ForEach(x => x.SetStartPosition());
        }

        public void DeactivateColliders()
        {
            _colliders.ForEach(x => x.enabled = false);
        }

        public virtual void Activate()
        {
            print(this);
        }
    }
}