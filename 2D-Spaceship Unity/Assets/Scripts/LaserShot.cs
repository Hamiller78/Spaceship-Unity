using System;
using UnityEngine;

public partial class LaserShot : MonoBehaviour
{
	[SerializeField]
	private double _lifetimeInSeconds = 1.5d;

	[SerializeField]
	public float Speed = 10f;

	private Vector2 _velocity;

	private double _remainingLifetime;

	// Called when the node enters the scene tree for the first time.
	public void Start()
	{
		_remainingLifetime = _lifetimeInSeconds;
		// GetNode<AudioStreamPlayer2D>("LaserSound").Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void Update()
	{
		var delta = Time.deltaTime;

		_remainingLifetime -= delta;
		if (_remainingLifetime <= 0d)
		{
			Destroy(gameObject);
			return;
		}
		transform.position += (Vector3)_velocity * (float)delta;
	}

	public void Initialize(Vector2 velocity)
	{
		_velocity = velocity;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Check if the laser hit an enemy
		if (collision.CompareTag("Enemy"))
		{
			Debug.Log("Laser hit an enemy!");

			// Destroy the enemy (or call a method on the enemy script)
			// TODO: Implement enemy destruction logic
			Destroy(collision.gameObject);

			// Destroy the laser
			Destroy(gameObject);
		}
		else if (collision.CompareTag("Obstacle"))
		{
			Debug.Log("Laser hit an obstacle!");

			// Destroy the laser
			Destroy(gameObject);
		}
	}
}
