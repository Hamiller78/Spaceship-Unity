using SpaceGame.Sprites;
using System;
using UnityEngine;


namespace SpaceGame
{
	public class SceneManager : MonoBehaviour
	{
		// [Export]
		// public PackedScene PlayerShipScene { get; set; }

		// [Export]
		// public PackedScene EnemyShipScene { get; set; }

		// [Export]
		// public PackedScene ExplosionScene { get; set; }

		// [Export]
		// public PackedScene TurretScene { get; set; }

		public GameObject TurretPrefab { get; set; }

		// [Export]
		// public PackedScene BossShipScene { get; set; }

		// [Export]
		// public PackedScene ExplosionScene { get; set; }

		// [Signal]
		// public delegate void PrintDebugMessageEventHandler(string debugMessage);

		private const int TURRET_COUNT = 10;

		private int _score = 0;
		private int _turretsLeft = 0;

		// Called when the node enters the scene tree for the first time.
		protected void Start()
		{
			// _playerShip = GetNode<PlayerShip>("Player");
			SpawnTurrets();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		protected void Update()
		{
			var delta = Time.deltaTime;

			// var camera = Camera.main;
			// camera.Position = camera.position.LinearInterpolate(_playerShip.Position, 0.1f);

			// var closestEnemyPosition = GetFurthestTurret();
			// if (closestEnemyPosition is null)
			// {
			// 	return;
			// }

			// var deltaX = Math.Abs(_playerShip.Position.X - closestEnemyPosition.Value.X);
			// var deltaY = Math.Abs(_playerShip.Position.Y - closestEnemyPosition.Value.Y);
			// var screenSize = _playerShip.GetViewportRect().Size;
			// var zoomX = Math.Min(1f, 0.5f * screenSize.X / deltaX);
			// var zoomY = Math.Min(1f, 0.5f * screenSize.Y / deltaY);
			// var zoom = Math.Min(zoomX, zoomY);

			// camera.Zoom = new Godot.Vector2(zoom, zoom);
		}

		// public void OnTurretDestroyed(Turret turret)
		// {
		// 	var explosion = ExplosionScene.Instantiate<Explosion>();
		// 	explosion.Position = turret.Position;
		// 	AddChild(explosion);
		// 	_score++;
		// 	_turretsLeft--;
		// 	EmitSignal(SignalName.PrintDebugMessage, $"Score: {_score}");

		// 	if (_turretsLeft == 0)
		// 	{
		// 		SpawnTurrets();
		// 	}
		// }

		// public void OnPlayerShipDestroyed(PlayerShip playerShip)
		// {
		// 	var explosion = ExplosionScene.Instantiate<Explosion>();
		// 	explosion.Position = playerShip.Position;
		// 	AddChild(explosion);
		// }

		// public void OnEnemyShipDestroyed(EnemyShip enemyShip)
		// {
		// 	var explosion = ExplosionScene.Instantiate<Explosion>();
		// 	explosion.Position = enemyShip.Position;
		// 	AddChild(explosion);
		// }

		private void SpawnTurrets()
		{
			for (int i = 0; i < TURRET_COUNT; i++)
			{
				var screenSizeX = Screen.width * 2;
				var screenSizeY = Screen.height * 2;

				var turret = Instantiate(TurretPrefab);
				turret.transform.position = new Vector3(
					UnityEngine.Random.Range(0, screenSizeX),
					UnityEngine.Random.Range(0, screenSizeY),
					0
				);
			}
			_turretsLeft += TURRET_COUNT;
		}

		// private Godot.Vector2? GetFurthestTurret()
		// {
		// 	var playerShip = GetNode<PlayerShip>("Player");
		// 	var furthestDistance = 0f;
		// 	Turret furthestTurret = null;

		// 	foreach (var turretNode in GetTree().GetNodesInGroup("Turrets"))
		// 	{
		// 		var turret = turretNode as Turret;
		// 		var distance = (playerShip?.Position ?? turret.Position).DistanceTo(turret.Position);
		// 		if (distance > furthestDistance)
		// 		{
		// 			furthestDistance = distance;
		// 			furthestTurret = turret;
		// 		}
		// 	}

		// 	return furthestTurret?.Position;
		// }
	}
}