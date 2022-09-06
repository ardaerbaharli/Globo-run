using System.Collections.Generic;
using System.Linq;
using Extensions;
using Managers;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obstacles
{
    public class ObstacleSetup : MonoBehaviour
    {
        [SerializeField] public bool startOnAwake;

        [SerializeField] private bool hasJumper;
        [SerializeField] private bool hasJumpers;
        [SerializeField] private bool randomizeJumper;

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


        [SerializeField] public int scoreValue;
        [SerializeField] public float length;
        [SerializeField] protected float height;
        public List<Obstacle> _obstacles;
        public List<Moving> MovingObstacles;
        private List<Collider> _colliders;
        private List<Renderer> _renderers;
        protected bool _isActivated;

        public PooledObject pooledObject;
        private GameObject _jumper;
        private List<GameObject> _jumpers;

        private void Awake()
        {
            if (hasJumper)
                _jumper = transform.GetChildObjectWithTag("Jumper");
            if (hasJumpers)
                _jumpers = transform.GetChildObjectsWithTag("Jumper");

            var obstacleObjects = transform.GetChildObjectsWithTag("Obstacle");
            _obstacles = obstacleObjects.Select(x => x.GetComponent<Obstacle>()).ToList();

            var movingObjects = transform.GetChildObjectsWithTag("Moving");
            MovingObstacles = movingObjects.Select(x => x.GetComponent<Moving>()).ToList();

            _colliders = new List<Collider>();
            _colliders.AddRange(_obstacles.Select(x => x.GetComponent<Collider>()).ToList());
            _colliders.AddRange(MovingObstacles.Select(x => x.GetComponent<Collider>()).ToList());

            _renderers = _obstacles.Select(x => x.GetComponent<Renderer>()).ToList();
            _renderers.AddRange(MovingObstacles.Select(x => x.GetComponent<Renderer>()));
        }

        private void OnEnable()
        {
            if (hasJumper && randomizeJumper) SetJumperPosition(_jumper);
            if (hasJumpers && randomizeJumper) SetJumpersPosition();
            _colliders.ForEach(x => x.enabled = true);
        }

        private void SetJumpersPosition()
        {
            _jumpers.ForEach(SetJumperPosition);
        }

        private void SetJumperPosition(GameObject jumper)
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

            FadeIn();
            _isActivated = false;

            ObjectPool.Instance.TakeBack(pooledObject);
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = new Vector3(pos.x, height, pos.z);
            MovingObstacles.ForEach(x => x.SetStartPosition());
        }

        public void DeactivateColliders()
        {
            _colliders.ForEach(x => x.enabled = false);
            FadeOut();
        }

        public virtual void Activate()
        {
            if (_isActivated) return;
            _isActivated = true;
        }

        public void FadeOut()
        {
            foreach (var r in _renderers)
            {
                var newColor = r.material.color;
                newColor.a = 0.2f;
                r.material.color = newColor;
                r.material.MakeFade();
            }
        }

        public void FadeIn()
        {
            foreach (var r in _renderers)
            {
                Color newColor = r.material.color;
                newColor.a = 1;
                r.material.color = newColor;
                r.material.MakeOpaque();
            }
        }
    }
}