using SpaceGame.Sprites;
using SpaceGame.Utilities;
using UnityEngine;

namespace SpaceGame.Sprites
{
    public class Turret : ShipBase
    {
        public delegate void TurretDestroyedEventHandler(Turret turret);

        public float FireRange { get; set; }

        public float ViewRange { get; set; }

        public float StartRotationDegrees { get; set; }

        public float MinRotationDegrees { get; set; }

        public float MaxRotationDegrees { get; set; }

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
            if (targetDistance < ViewRange)
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
            // _targetPosition = position;
            // _angleToTarget = NavigationManager.GetGlobalAngleToTarget(
            //     transform.position,
            //     _targetPosition,
            //     transform.eulerAngles.z - RotationDegrees
            // );
        }

        private void TurnTurret(float delta)
        {
            // var newRotation = NavigationManager.GetNewRotation(
            //     RotationDegrees,
            //     _angleToTarget.InDegrees,
            //     TurnRateDegreesPerSecond,
            //     MinRotationDegrees,
            //     MaxRotationDegrees,
            //     delta
            // );
            // DeltaRotation = new Angle(0f); // TODO: Remove this when it is no longer used in base class
            // RotationDegrees = newRotation;
        }
    }
}