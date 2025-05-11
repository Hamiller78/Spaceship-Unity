using System;
using UnityEngine;

namespace SpaceGame.Sprites
{
	public class Explosion : MonoBehaviour
	{
		// Called when the node enters the scene tree for the first time.
		public void Start()
		{
			Debug.Log("Explosion started");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public void Update()
		{
			var delta = Time.deltaTime;
		}

		public void OnAnimationFinished()
		{
			Debug.Log("Explosion animation finished");
			Destroy(gameObject);
		}

		public void OnSoundFinished()
		{
			// 	_isSoundFinished = true;
			// 	if (_isAnimationFinished)
			// 	{
			// 		Destroy(gameObject);
			// 	}
			// }
		}
	}
}