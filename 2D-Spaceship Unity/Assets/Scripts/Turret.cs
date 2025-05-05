using SpaceGame.Sprites;
using SpaceGame.Utilities;
using UnityEngine;

namespace SpaceGame.Sprites
{
    public class Turret : ShipBase
    {
        public delegate void TurretDestroyedEventHandler(Turret turret);

        public float FireRange { get; set; }

        [SerializeField]
        private float _viewRange;

        public float StartRotationDegrees { get; set; } = 0f;

        public float MinRotationDegrees { get; set; } = 0f;

        public float MaxRotationDegrees { get; set; } = 360f;

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

            if (targetDistance < FireRange)
            {
                //                FirePrimary();
            }

            base.Update();
        }

        public void OnTargetPositionUpdated(Vector2 position, Vector2 velocity)
        {
            Debug.Log($"Turret: Target position updated to {position} with velocity {velocity}");
            _targetPosition = position;
            _angleToTarget = NavigationManager.GetAngleToTarget(
                transform.position,
                _targetPosition
            );
            Debug.Log($"Turret: Angle to target is {_angleToTarget.InDegrees} degrees");
        }

        private void TurnTurret(float delta)
        {
            Debug.Log($"Old rotation: {transform.eulerAngles.z} degrees, Angle to target: {_angleToTarget.InDegrees} degrees");
            var newRotation = NavigationManager.GetNewRotation(
                transform.eulerAngles.z,
                _angleToTarget.InDegrees,
                TurnRateDegreesPerSecond,
                MinRotationDegrees,
                MaxRotationDegrees,
                delta
            );
            Debug.Log($"New rotation: {newRotation} degrees");
            DeltaRotation = new Angle(0f); // TODO: Remove this when it is no longer used in base class
            transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }
    }
}