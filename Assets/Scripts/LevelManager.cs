using System.Collections.Generic;
using Obstacles;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float zDif;
    [SerializeField] private Vector3 obstacleStartPoint;
    [SerializeField] private Vector3 planeStartPoint;
    [SerializeField] private int numberOfInitialObstacles;
    [SerializeField] private float planeLength;

    private int _obstacleIndex;
    private int _planeIndex;
    private Vector3 _lastPos;
    private float _previousLength;
    private List<ObstacleSetup> _obstacles;
    private ObstacleSetup NextObstacle => _obstacles[ScoreManager.instance.Score];
    private ObstacleSetup _currentObstacle, _previousObstacle, _prePreviousObstacle;

    private PowerUpSpawner _powerUpSpawner;
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _powerUpSpawner = GetComponent<PowerUpSpawner>();
        _obstacles = new List<ObstacleSetup>();
    }

    private void Start()
    {
        ScoreManager.instance.OnScored += OnScored;
    }

    public void Init()
    {
        for (var i = 0; i < numberOfInitialObstacles; i++)
        {
            SpawnObstacle();
        }

        _powerUpSpawner.Init();

        _currentObstacle = NextObstacle;
        _currentObstacle.gameObject.SetActive(true);
        _currentObstacle.Activate();
    }


    private void OnScored(int score)
    {
        _prePreviousObstacle = _previousObstacle;
        _previousObstacle = _currentObstacle;
        _currentObstacle = NextObstacle;

        _currentObstacle.gameObject.SetActive(true);
        _currentObstacle.Activate();

        if (_prePreviousObstacle is not null)
            _prePreviousObstacle.TakeBackToPool();

        SpawnObstacle();
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