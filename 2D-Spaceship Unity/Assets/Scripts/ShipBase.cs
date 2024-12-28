using SpaceGame.Utilities;
using static System.Math;
using UnityEngine;

namespace SpaceGame.Sprites
{
    public class ShipBase : MonoBehaviour
    {
        public GameObject EngineFlameAnimation; // Reference to the flame GameObject
        public float MaxAcceleration { get; set; } = 1f;

        public float TurnRateDegreesPerSecond { get; set; } = 90f;

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
            var currentRotation = transform.rotation.eulerAngles.z;
            var newRotation = new Angle(currentRotation) + DeltaRotation;
            transform.rotation = Quaternion.Euler(0, 0, newRotation.InDegrees);

            var vx = (float)(_velocity.x + Cos(newRotation.InRadians) * DeltaVelocity);
            var vy = (float)(_velocity.y + Sin(newRotation.InRadians) * DeltaVelocity);
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