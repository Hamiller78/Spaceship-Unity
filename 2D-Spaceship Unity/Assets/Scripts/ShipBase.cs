using SpaceGame.Utilities;
using static System.Math;
using UnityEngine;

namespace SpaceGame.Sprites
{
    public class ShipBase : MonoBehaviour
    {
        public delegate void PositionUpdatedHandler(Vector2 position, Vector2 velocity);
        public event PositionUpdatedHandler PositionUpdated;

        [SerializeField]
        private GameObject _laserShotPrefab;

        [SerializeField]
        private float _rechargeTime = 0.5f;

        public GameObject EngineFlameAnimation; // Reference to the flame GameObject
        public float MaxAcceleration { get; set; } = 1f;

        public float TurnRateDegreesPerSecond { get; set; } = 90f;

        protected float DeltaSpeed;
        protected Angle DeltaRotation = new();
        protected float DeltaVelocity;
        protected Vector3 _velocity = new Vector3(0, 0, 0);

        protected bool IsEngineRunning = false;
        private float _rechargeTimeRemaining = -1f;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Start()
        {

        }

        protected void NotifyPositionUpdated(Vector2 position, Vector2 velocity)
        {
            // Safely invoke the event
            PositionUpdated?.Invoke(position, velocity);
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

            if (newPosition != transform.localPosition)
            {
                NotifyPositionUpdated(newPosition, _velocity);
                transform.localPosition = newPosition;
            }

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

            if (_rechargeTimeRemaining > 0f)
            {
                _rechargeTimeRemaining -= (float)Time.deltaTime;
            }
        }

        protected void FirePrimary()
        {
            if (_rechargeTimeRemaining > 0f)
            {
                return;
            }

            Debug.Log("Firing primary weapon!");
            var newShotObject = Instantiate(_laserShotPrefab);
            var newShot = newShotObject.GetComponent<LaserShot>();

            float rotationInRadians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            newShot.transform.position
                = transform.position
                    + new Vector3(
                        0.15f * (float)Cos(rotationInRadians),
                        0.15f * (float)Sin(rotationInRadians),
                        0f);
            newShot.transform.rotation = transform.rotation;

            Vector2 velocity = (Vector2)_velocity
            + new Vector2(
                Mathf.Cos(rotationInRadians) * newShot.Speed,
                Mathf.Sin(rotationInRadians) * newShot.Speed
            );
            newShot.Initialize(velocity);

            _rechargeTimeRemaining = _rechargeTime;
        }
    }
}