using System;
using System.Collections.Generic;
using Obstacles;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private float zDif;
        [SerializeField] private Vector3 obstacleStartPoint;
        [SerializeField] private Vector3 planeStartPoint;
        [SerializeField] private int numberOfInitialObstacles;
        [SerializeField] private float planeLength;

        public Action OnObstaclePassed;
        private int _obstacleIndex;
        private int _planeIndex;
        private Vector3 _lastPos;
        private float _previousLength;
        private List<ObstacleSetup> _obstacles;
        private ObstacleSetup NextObstacle => _obstacles[passedObstacles];
        private ObstacleSetup _currentObstacle, _previousObstacle, _prePreviousObstacle;

        private CollectableSpawner _collectableSpawner;
        public static LevelManager Instance;
        private int passedObstacles;

        public int PassedObstacles
        {
            get { return passedObstacles; }
            set
            {
                passedObstacles = value;
                PassedObstacle();
                OnObstaclePassed?.Invoke();
            }
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _collectableSpawner = GetComponent<CollectableSpawner>();
            _obstacles = new List<ObstacleSetup>();
        }


        public void Init()
        {
            for (var i = 0; i < numberOfInitialObstacles; i++)
            {
                SpawnObstacle();
            }

            _collectableSpawner.Init();
            _currentObstacle = NextObstacle;
            SetObstaclesActive();
            _currentObstacle.Activate();
        }

        private void PassedObstacle()
        {
            ScoreManager.instance.Score += _currentObstacle.scoreValue;
            Vibration.Light();
            OnObstaclePassed?.Invoke();

            _prePreviousObstacle = _previousObstacle;
            _previousObstacle = _currentObstacle;
            _currentObstacle = NextObstacle;

            _currentObstacle.Activate();
            _obstacles[passedObstacles + 2].gameObject.SetActive(true);

            SetObstaclesActive();
            if (_prePreviousObstacle is not null)
                _prePreviousObstacle.TakeBackToPool();

            SpawnObstacle();
        }

        private void SetObstaclesActive()
        {
            // _obstacles[passedObstacles].gameObject.SetActive(true);
            // _obstacles[passedObstacles + 1].gameObject.SetActive(true);
            // _obstacles[passedObstacles + 2].gameObject.SetActive(true);

            for (var i = passedObstacles; i < passedObstacles + 2; i++)
            {
                var obstacle = _obstacles[i];
                obstacle.gameObject.SetActive(true);
                if (obstacle.startOnAwake || i == passedObstacles)
                    obstacle.Activate();
            }
        }


        private void SpawnObstacle()
        {
            var obstaclePooledObject = ObjectPool.Instance.GetObstaclePooledObject();
            var obstacle = obstaclePooledObject.gameObject.GetComponent<ObstacleSetup>();
            obstacle.pooledObject = obstaclePooledObject;


            var length = obstacle.length;
            var pos = _lastPos + (length / 2f + _previousLength / 2f + zDif) * Vector3.forward;
            _previousLength = length;
            _lastPos = pos;
            obstacle.SetPosition(pos);

            var planeObj = ObjectPool.Instance.GetPooledObject("Plane");
            planeObj.transform.position = planeStartPoint + new Vector3(0, 0, planeLength * _planeIndex);
            var plane = planeObj.gameObject.GetComponent<Plane>();
            plane.pooledObject = planeObj;
            _planeIndex++;
            planeObj.gameObject.SetActive(true);

            // obstacle.plane = planeObj;

            // var planeScale =planeObj.transform.localScale;
            // planeScale.z

            // var p = obstacleStartPoint + _obstacleIndex * zDif * Vector3.forward;
            // obstacle.SetPosition(p);
            obstaclePooledObject.gameObject.name = "Level" + (_obstacleIndex + 1);

            _obstacles.Add(obstacle);
            _obstacleIndex++;
        }
    }
}