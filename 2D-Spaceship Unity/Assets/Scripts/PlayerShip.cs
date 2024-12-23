using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public GameObject EngineFlameAnimation; // Reference to the flame GameObject
    private Animator _engineFlameAnimationController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (EngineFlameAnimation != null)
        {
            _engineFlameAnimationController = EngineFlameAnimation.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float thrustInput = Input.GetAxis("Vertical");

        if (thrustInput != 0f)
        {
            // If the player is pressing the thrust button, play the flame EngineFlameAnimation
            EngineFlameAnimation.SetActive(true);
        }
        else
        {
            EngineFlameAnimation.SetActive(false);
        }
    }
}
