using SpaceGame.Utilities;
using UnityEngine;

namespace SpaceGame.Sprites
{
    public class PlayerShip : ShipBase
    {
        private Animator _engineFlameAnimationController;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        new void Start()
        {
            if (EngineFlameAnimation != null)
            {
                _engineFlameAnimationController = EngineFlameAnimation.GetComponent<Animator>();
            }

            base.Start();
        }

        // Update is called once per frame
        new void Update()
        {
            var delta = Time.deltaTime;

            var thrustInput = Input.GetAxis("Vertical");
            if (thrustInput != 0f)
            {
                DeltaVelocity = MaxAcceleration * (float)delta;
                EngineFlameAnimation.SetActive(true);
            }
            else
            {
                DeltaVelocity = 0f;
                EngineFlameAnimation.SetActive(false);
            }

            var steeringInput = Input.GetAxis("Horizontal");
            DeltaRotation = new Angle(-steeringInput * TurnRateDegreesPerSecond * (float)delta);

            var fireInput = Input.GetButton("Fire1");
            if (fireInput)
            {
                Debug.Log("PlayerShip: Fire button pressed");
                FirePrimary();
            }

            base.Update();
        }
    }
}
