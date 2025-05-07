using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{

	private bool _isAnimationFinished = false;
	private bool _isSoundFinished = false;

	// Called when the node enters the scene tree for the first time.
	public void Start()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void Update()
	{
		var delta = Time.deltaTime;
	}

	public void OnAnimationFinished()
	{
		_isAnimationFinished = true;
		if (_isSoundFinished)
		{
			Destroy(gameObject);
		}
	}

	public void OnSoundFinished()
	{
		_isSoundFinished = true;
		if (_isAnimationFinished)
		{
			Destroy(gameObject);
		}
	}
}
