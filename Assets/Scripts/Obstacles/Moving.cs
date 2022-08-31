using System;
using Enums;
using UnityEngine;
using Utilities;
using static Utilities.iTween;
using Random = UnityEngine.Random;

namespace Obstacles
{
    public class Moving : MonoBehaviour
    {
        [NonSerialized] public float MoveDistance;
        [NonSerialized] public float Interval;
        [NonSerialized] public Direction Direction;
        [SerializeField] private Axis moveAxis;
        [SerializeField] private EaseType _easeType;
        [SerializeField] private LoopType _loopType;

        private float _moveDistance;
        private string _moveAxis;

        private Vector3 _startPosition;

        public void SetStartPosition()
        {
            _startPosition = transform.localPosition;
        }

        public void ResetPosition()
        {
            transform.localPosition = _startPosition;
        }

        public void Activate()
        {
            _moveDistance = Direction switch
            {
                Direction.LeftToRight => Mathf.Abs(MoveDistance),
                Direction.Upwards => Mathf.Abs(MoveDistance),
                Direction.RightToLeft => -Mathf.Abs(MoveDistance),
                Direction.Downwards => -Mathf.Abs(MoveDistance),
                _ => _moveDistance
            };

            _moveAxis = moveAxis switch
            {
                Axis.X => "x",
                Axis.Y => "y",
                _ => "z"
            };
            
            MoveBy(gameObject,
                Hash(_moveAxis, _moveDistance, "easeType", _easeType, "loopType", _loopType, "time",
                    Interval));
        }

        private void Awake()
        {
            var renderer = GetComponent<Renderer>();
            var num = Random.Range(0, 5);
            renderer.material = Resources.Load<Material>("Materials/" + num);
        }


        private void Start()
        {
            GameManager.instance.OnPaused += OnPaused;
            GameManager.instance.OnResumed += OnResumed;
        }

        private void OnResumed()
        {
            Resume(gameObject);
        }

        private void OnPaused()
        {
            Pause(gameObject);
        }
    }
}