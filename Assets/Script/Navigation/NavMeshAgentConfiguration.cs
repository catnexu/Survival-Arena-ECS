using System;
using UnityEngine;
using UnityEngine.AI;

namespace Navigation
{
    [Serializable]
    public struct NavMeshAgentConfiguration : IEquatable<NavMeshAgentConfiguration>
    {
        [SerializeField, NavMeshAgent] private int _agentTypeId;
        [SerializeField] private float _baseOffset;

        [SerializeField, Header("Steering")] private float _speed;
        [SerializeField] private float _angularSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _stoppingDistance;
        [SerializeField] private bool _autoBraking;

        [SerializeField, Header("Obstacle Avoidance")]
        private float _radius;

        [SerializeField] private float _height;
        [SerializeField] private ObstacleAvoidanceType _quality;
        [SerializeField, Range(0, 99)] private int _agentPriority;

        [SerializeField, Header("Path Finding")]
        private bool _autoTraverseOffMeshLink;

        [SerializeField] private bool _autoRepath;
        [SerializeField, NavMeshAreaMask] private int _areaMask;
        public int AgentTypeId => _agentTypeId;
        public float BaseOffset => _baseOffset;
        public float Speed => _speed;
        public float AngularSpeed => _angularSpeed;
        public float Acceleration => _acceleration;
        public float StoppingDistance => _stoppingDistance;
        public bool AutoBraking => _autoBraking;
        public float Radius => _radius;
        public float Height => _height;
        public ObstacleAvoidanceType Quality => _quality;
        public int AgentPriority => _agentPriority;
        public bool AutoTraverseOffMeshLink => _autoTraverseOffMeshLink;
        public bool AutoRepath => _autoRepath;
        public int AreaMask => _areaMask;

        public NavMeshAgentConfiguration(int agentTypeId, int areaMask, float radius, float height)
        {
            _agentTypeId = agentTypeId;
            _baseOffset = 0;
            _speed = 0;
            _angularSpeed = 0;
            _acceleration = 10;
            _stoppingDistance = 1;
            _autoBraking = true;
            _radius = radius;
            _height = height;
            _quality = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
            _agentPriority = 50;
            _autoTraverseOffMeshLink = false;
            _autoRepath = false;
            _areaMask = areaMask;
        }

        public NavMeshAgentConfiguration(int agentTypeId, float baseOffset, float speed, float angularSpeed, float acceleration,
            float stoppingDistance, bool autoBraking, float radius, float height, ObstacleAvoidanceType quality, int agentPriority,
            bool autoTraverseOffMeshLink, bool autoRepath, int areaMask)
        {
            _agentTypeId = agentTypeId;
            _baseOffset = baseOffset;
            _speed = speed;
            _angularSpeed = angularSpeed;
            _acceleration = acceleration;
            _stoppingDistance = stoppingDistance;
            _autoBraking = autoBraking;
            _radius = radius;
            _height = height;
            _quality = quality;
            _agentPriority = agentPriority;
            _autoTraverseOffMeshLink = autoTraverseOffMeshLink;
            _autoRepath = autoRepath;
            _areaMask = areaMask;
        }

        public bool Equals(NavMeshAgentConfiguration other)
        {
            return _agentTypeId == other._agentTypeId && _baseOffset.Equals(other._baseOffset) && _angularSpeed.Equals(other._angularSpeed) &&
                _acceleration.Equals(other._acceleration) && _stoppingDistance.Equals(other._stoppingDistance) &&
                _autoBraking == other._autoBraking && _radius.Equals(other._radius) && _height.Equals(other._height) && _quality == other._quality &&
                _agentPriority == other._agentPriority && _autoTraverseOffMeshLink == other._autoTraverseOffMeshLink &&
                _autoRepath == other._autoRepath && _areaMask == other._areaMask;
        }

        public override bool Equals(object obj)
        {
            return obj is NavMeshAgentConfiguration other && Equals(other);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();
            hashCode.Add(_agentTypeId);
            hashCode.Add(_baseOffset);
            hashCode.Add(_angularSpeed);
            hashCode.Add(_acceleration);
            hashCode.Add(_stoppingDistance);
            hashCode.Add(_autoBraking);
            hashCode.Add(_radius);
            hashCode.Add(_height);
            hashCode.Add((int) _quality);
            hashCode.Add(_agentPriority);
            hashCode.Add(_autoTraverseOffMeshLink);
            hashCode.Add(_autoRepath);
            hashCode.Add(_areaMask);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(NavMeshAgentConfiguration left, NavMeshAgentConfiguration right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NavMeshAgentConfiguration left, NavMeshAgentConfiguration right)
        {
            return !left.Equals(right);
        }
    }
}