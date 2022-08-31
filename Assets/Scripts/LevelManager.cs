using System.Collections.Generic;
using Obstacles;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float zDif;
    [SerializeField] private Vector3 obstacleStartPoint;
    [SerializeField] private Vector3 planeStartPoint;
    [SerializeField] private int numberOfInitialObstacles;
    [SerializeField] private int numberOfInitialPlanes;
    [SerializeField] private float planeLength;

    private int _obstacleIndex;
    private int _planeIndex;
    private List<ObstacleSetup> _obstacles;
    private List<GameObject> _planes;
    private ObstacleSetup nextObstacle => _obstacles[ScoreManager.instance.Score];
    private ObstacleSetup currentObstacle, previousObstacle, prePreviousObstacle;

    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        _obstacles = new List<ObstacleSetup>();
        _planes = new List<GameObject>();
    }

    private void Start()
    {
        ScoreManager.instance.OnScored += OnScored;
    }

    public void Init()
    {
        for (var i = 0; i < numberOfInitialObstacles; i++)
        {
            LoadObstacle();
        }

        currentObstacle = nextObstacle;
        currentObstacle.gameObject.SetActive(true);
        currentObstacle.Activate();
    }


    private void OnScored(int score)
    {
        prePreviousObstacle = previousObstacle;
        previousObstacle = currentObstacle;
        currentObstacle = nextObstacle;

        currentObstacle.gameObject.SetActive(true);
        currentObstacle.Activate();
        
        if (prePreviousObstacle is not null)
            prePreviousObstacle.TakeBackToPool();

        LoadObstacle();
    }

    private void LoadObstacle()
    {
        var obstaclePooledObject = ObjectPool.instance.GetObstaclePooledObject();
        var obstacle = obstaclePooledObject.gameObject.GetComponent<ObstacleSetup>();
        obstacle.pooledObject = obstaclePooledObject;

        var plane = ObjectPool.instance.GetPooledObject("Plane");
        plane.transform.position = planeStartPoint + new Vector3(0, 0, planeLength * _planeIndex);
        _planeIndex++;
        _planes.Add(plane.gameObject);
        obstacle.plane = plane;
        plane.gameObject.SetActive(true);

        var p = obstacleStartPoint + _obstacleIndex * zDif * Vector3.forward;
        obstacle.SetPosition(p);
        obstaclePooledObject.gameObject.name = "Level" + (_obstacleIndex + 1);

        _obstacles.Add(obstacle);
        _obstacleIndex++;
    }
}