using SpaceGame.Utilities;
using static System.Math;
using UnityEngine;

namespace SpaceGame.Sprites
{
    public class ShipBase : MonoBehaviour
    {
        public GameObject EngineFlameAnimation; // Reference to the flame GameObject
        public float MaxAcceleration { get; set; } = 1f;

        public float TurnRateDegreesPerSecond { get; set; }

        protected float DeltaSpeed;
        protected Angle DeltaRotation = new();
        protected float DeltaVelocity;
        private Vector3 _velocity = new Vector3(0, 0, 0);

        protected bool IsEngineRunning = false;
        private float _rechargeTimeRemaining = 0f;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Start()
        {

        }

        // Update is called once per frame
        protected void Update()
        {
            var delta = Time.deltaTime;

            // Movement
            // var newRotation = new Angle(RotationDegrees) + DeltaRotation;
            // RotationDegrees = newRotation.InDegrees;

            // var vx = (float)(_velocity.x + Cos(transform.rotation) * DeltaVelocity);
            // var vy = (float)(_velocity.y + Sin(transform.rotation) * DeltaVelocity);
            var vx = (float)(_velocity.x + DeltaVelocity);
            var vy = (float)(_velocity.y);
            _velocity = new Vector2(vx, vy);

            var newX = (float)(transform.localPosition.x + _velocity.x * delta);
            var newY = (float)(transform.localPosition.y + _velocity.y * delta);
            var newPosition = new Vector3(newX, newY, 0);
            transform.localPosition = newPosition;

            // EmitSignal(SignalName.PositionUpdated, newPosition, _velocity);

            // Sound & Engine flame
            if (!IsEngineRunning && DeltaVelocity > 0f)
            {
                IsEngineRunning = true;
                if (EngineFlameAnimation is not null)
                {
                    EngineFlameAnimation.SetActive(true);
                }
            }
            else
            {
                if (IsEngineRunning && DeltaVelocity == 0f)
                {
                    IsEngineRunning = false;
                    if (EngineFlameAnimation is not null)
                    {
                        EngineFlameAnimation.SetActive(false);
                    }
                }
            }

            // if (_rechargeTimeRemaining > 0f)
            // {
            //     _rechargeTimeRemaining -= (float)Time.deltaTime;
            // }
        }
    }
}