using SpaceGame.Sprites;
using SpaceGame.Utilities;
using UnityEngine;

namespace SpaceGame.Sprites
{
    public class Turret : ShipBase
    {
        public delegate void TurretDestroyedEventHandler(Turret turret);

        [SerializeField]
        private float _fireRange;

        [SerializeField]
        private float _viewRange;

        public float StartRotationDegrees { get; set; } = 0f;

        public float MinRotationDegrees { get; set; } = 0f;

        public float MaxRotationDegrees { get; set; } = 360f;
        private ShipBase _targetShip;
        private Vector2 _targetPosition;
        private Angle _angleToTarget = new();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        new void Start()
        {
            transform.rotation = Quaternion.Euler(0, 0, StartRotationDegrees);
            base.Start();
        }

        // Update is called once per frame
        new void Update()
        {
            DeltaRotation = new Angle(0f);

            var targetDistance = Vector2.Distance(_targetPosition, transform.position);
            if (targetDistance < _viewRange)
            {
                TurnTurret(Time.deltaTime);
            }

            if (targetDistance < _fireRange)
            {
                FirePrimary();
            }

            base.Update();
        }

        public void Initialize(ShipBase targetShip)
        {
            _targetShip = targetShip;

            // Subscribe to the PositionUpdated event
            if (_targetShip != null)
            {
                _targetShip.PositionUpdated += OnTargetPositionUpdated;
            }
        }

        public void OnTargetPositionUpdated(Vector2 position, Vector2 velocity)
        {
            _targetPosition = position;
            _angleToTarget = NavigationManager.GetAngleToTarget(
                transform.position,
                _targetPosition
            );
        }

        public override void OnDestroy()
        {
            // Unsubscribe from the PositionUpdated event
            if (_targetShip != null)
            {
                _targetShip.PositionUpdated -= OnTargetPositionUpdated;
            }

            base.OnDestroy();
        }

        private void TurnTurret(float delta)
        {
            var newRotation = NavigationManager.GetNewTurretRotation(
                transform.eulerAngles.z,
                _angleToTarget.InDegrees,
                TurnRateDegreesPerSecond,
                MinRotationDegrees,
                MaxRotationDegrees,
                delta
            );
            DeltaRotation = new Angle(0f); // TODO: Remove this when it is no longer used in base class
            transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }
    }
}
