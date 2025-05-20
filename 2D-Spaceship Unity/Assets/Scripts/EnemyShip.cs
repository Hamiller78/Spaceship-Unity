using System;
using UnityEngine;

namespace SpaceGame.Sprites
{
    public class EnemyShip : ShipBase
    {
        private enum ManeuverMode
        {
            None,
            Approach,
            Attack,
        }

        [SerializeField]
        public float Acceleration { get; set; } = 200f;

        [SerializeField]
        public float RadsPerSecond { get; set; } = 2f * (float)Math.PI * 0.4f;

        [SerializeField]
        public GameObject LaserShotPrefab { get; set; }

        [SerializeField]
        public float RechargeTime { get; set; } = 0.5f;

        [SerializeField]
        private ShipBase _targetShip;

        public float FireRange { get; set; } = 700f;

        private Vector2 _targetPosition;
        private Vector2 _targetVelocity;
        private float _targetRotation;
        private ManeuverMode _maneuverMode = ManeuverMode.None;

        public Vector2 Velocity
        {
            get => _velocity;
        }

        private bool _isEngineRunning = false;

        // Called when the node enters the scene tree for the first time.
        protected override void Start()
        {
            _maneuverMode = ManeuverMode.Approach;
            _targetShip.PositionUpdated += OnTargetPositionUpdated;

            base.Start();
        }

        protected override void Update()
        {
            var delta = Time.deltaTime;

            var targetDistance = Vector2.Distance(_targetPosition, transform.position);
            if (_maneuverMode == ManeuverMode.Approach && targetDistance <= FireRange)
            {
                _maneuverMode = ManeuverMode.Attack;
            }
            else if (_maneuverMode == ManeuverMode.Attack && targetDistance > FireRange + 200d)
            {
                _maneuverMode = ManeuverMode.Approach;
            }

            switch (_maneuverMode)
            {
                case ManeuverMode.Approach:
                    PerformApproach(delta);
                    break;
                case ManeuverMode.Attack:
                    PerformAttack(delta);
                    break;
                default:
                    break;
            }

            var newX = (float)(transform.position.x + _velocity.x * delta);
            var newY = (float)(transform.position.y + _velocity.y * delta);

            var newPosition = new Vector2(newX, newY);
            transform.position = newPosition;

            base.Update();
        }

        private void PerformAttack(double delta)
        {
            TurnToTarget(delta);

            if (Math.Abs(GetDeltaAngleToTarget()) < Math.PI / 4d)
            {
                FirePrimary();
                StopEngine();
            }
        }

        private void PerformApproach(double delta)
        {
            var deltaPos =
                _targetPosition - new Vector2(transform.position.x, transform.position.y);

            var desiredDeltaV =
                deltaPos.normalized * (float)Math.Sqrt(Acceleration * deltaPos.magnitude);
            var deltaVDelta = new Vector3(desiredDeltaV.x, desiredDeltaV.y) - _velocity;

            // Unity translation of Godot velocity update
            var shipRotation = transform.eulerAngles.z * Mathf.Deg2Rad;
            var rotationDelta =
                Vector2.SignedAngle(Vector2.right, new Vector2(deltaVDelta.x, deltaVDelta.y))
                    * Mathf.Deg2Rad
                - shipRotation;
            if (Math.Abs(rotationDelta) < Math.PI / 6f)
            {
                RunEngine(delta);
            }
            else
            {
                StopEngine();
            }
            // Godot: Rotation += Math.Sign(rotationDelta) * RadsPerSecond * (float)delta;
            // Unity equivalent:
            transform.rotation = Quaternion.Euler(
                0f,
                0f,
                transform.rotation.eulerAngles.z
                    + Mathf.Sign(rotationDelta) * Mathf.Rad2Deg * RadsPerSecond * (float)delta
            );
        }

        public void OnTargetPositionUpdated(Vector2 position, Vector2 velocity)
        {
            _targetPosition = position;
            _targetVelocity = velocity;
            // Replace this Godot code:
            // _targetRotation = Position.AngleToPoint(_targetPosition);

            // With this Unity equivalent:
            var direction = _targetPosition - (Vector2)transform.position;
            _targetRotation = Mathf.Atan2(direction.y, direction.x);
        }

        private void TurnToTarget(double delta)
        {
            // Godot: var rotationDelta = _targetRotation - (Rotation - (float)Math.PI / 2f);
            // Unity equivalent:
            var rotationDelta = _targetRotation - (transform.eulerAngles.z * Mathf.Deg2Rad);
            if (rotationDelta < -Math.PI)
            {
                rotationDelta += (float)(2d * Math.PI);
            }
            else if (rotationDelta > Math.PI)
            {
                rotationDelta -= (float)(2d * Math.PI);
            }
            var rotationStep = RadsPerSecond * (float)delta;

            if (Math.Abs(rotationDelta) <= rotationStep)
            {
                // Unity: set the rotation directly using Quaternion.Euler
                transform.rotation = Quaternion.Euler(0f, 0f, _targetRotation * Mathf.Rad2Deg);
            }
            else
            {
                // Unity: incrementally rotate towards the target
                float newZ =
                    transform.rotation.eulerAngles.z
                    + Mathf.Sign(rotationDelta) * rotationStep * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, newZ);
            }
        }

        private void RunEngine(double delta)
        {
            if (!_isEngineRunning)
            {
                _isEngineRunning = true;
                //GetNode<AnimatedSprite2D>("ShipSprite/EngineFlame").Visible = true;
            }

            var shipRotation = transform.rotation.eulerAngles.z;
            var vx =
                _velocity.x + Mathf.Cos(shipRotation * Mathf.Deg2Rad) * Acceleration * (float)delta;
            var vy =
                _velocity.y + Mathf.Sin(shipRotation * Mathf.Deg2Rad) * Acceleration * (float)delta;
            _velocity = new Vector2(vx, vy);
        }

        private void StopEngine()
        {
            if (_isEngineRunning)
            {
                _isEngineRunning = false;
                //GetNode<AnimatedSprite2D>("ShipSprite/EngineFlame").Visible = false;
            }
        }

        private float GetDeltaAngleToTarget()
        {
            return _targetRotation - transform.rotation.eulerAngles.z + (float)Math.PI / 2f;
        }

        private float GetShipRotation()
        {
            return transform.rotation.eulerAngles.z - (float)Math.PI / 2f;
        }
    }
}
