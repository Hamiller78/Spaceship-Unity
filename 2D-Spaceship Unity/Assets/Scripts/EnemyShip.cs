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
        private float _acceleration;

        [SerializeField]
        private float _turnRateInDegsPerSecond;

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

            Debug.Log($"Target distance: {targetDistance}");
            Debug.Log($"Maneuver mode: {_maneuverMode}");

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

            base.Update();
        }

        private void PerformAttack(double delta)
        {
            Debug.Log($"Performing attack with delta: {delta}");

            TurnToTarget(delta);

            Debug.Log($"Angle to target: {GetDeltaAngleToTarget()}");
            if (Math.Abs(GetDeltaAngleToTarget()) < 30f)
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
                deltaPos.normalized * (float)Math.Sqrt(_acceleration * deltaPos.magnitude);
            var deltaVDelta = new Vector3(desiredDeltaV.x, desiredDeltaV.y) - _velocity;

            // Unity translation of Godot velocity update
            var shipRotation = transform.eulerAngles.z;
            var rotationDelta =
                Vector2.SignedAngle(Vector2.right, new Vector2(deltaVDelta.x, deltaVDelta.y))
                - shipRotation;
            if (Math.Abs(rotationDelta) < 60f)
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
                    + Mathf.Sign(rotationDelta) * _turnRateInDegsPerSecond * (float)delta
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
            _targetRotation = Mathf.Atan2(direction.y, direction.x) + Mathf.Rad2Deg;
        }

        private void TurnToTarget(double delta)
        {
            // Godot: var rotationDelta = _targetRotation - (Rotation - (float)Math.PI / 2f);
            // Unity equivalent:
            var rotationDelta = _targetRotation - (transform.eulerAngles.z);
            if (rotationDelta < 180f)
            {
                rotationDelta += 360f;
            }
            else if (rotationDelta > 180f)
            {
                rotationDelta -= 360f;
            }
            var rotationStep = _turnRateInDegsPerSecond * (float)delta;

            if (Math.Abs(rotationDelta) <= rotationStep)
            {
                // Unity: set the rotation directly using Quaternion.Euler
                transform.rotation = Quaternion.Euler(0f, 0f, _targetRotation);
            }
            else
            {
                // Unity: incrementally rotate towards the target
                float newZ =
                    transform.rotation.eulerAngles.z + Mathf.Sign(rotationDelta) * rotationStep;
                transform.rotation = Quaternion.Euler(0f, 0f, newZ);
            }
        }

        private void RunEngine(double delta)
        {
            Debug.Log($"Running engine with delta: {delta}");

            if (!_isEngineRunning)
            {
                _isEngineRunning = true;
                //GetNode<AnimatedSprite2D>("ShipSprite/EngineFlame").Visible = true;
            }

            var shipRotation = transform.rotation.eulerAngles.z;
            var vx =
                _velocity.x
                + Mathf.Cos(shipRotation * Mathf.Deg2Rad) * _acceleration * (float)delta;
            var vy =
                _velocity.y
                + Mathf.Sin(shipRotation * Mathf.Deg2Rad) * _acceleration * (float)delta;
            _velocity = new Vector3(vx, vy, 0);
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
            return _targetRotation - transform.rotation.eulerAngles.z + 90f;
        }

        private float GetShipRotation()
        {
            return transform.rotation.eulerAngles.z - 90f;
        }
    }
}
