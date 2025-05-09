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

		[SerializeField]
		private GameObject _turretPrefab;

		[SerializeField]
		private GameObject target;

		// [Export]
		// public PackedScene BossShipScene { get; set; }

		// [Export]
		// public PackedScene ExplosionScene { get; set; }

		// [Signal]
		// public delegate void PrintDebugMessageEventHandler(string debugMessage);

		private const int TURRET_COUNT = 1;

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
			if (_turretPrefab == null)
			{
				Debug.LogError("TurretPrefab is not assigned in the SceneManager!");
				return;
			}

			if (target == null)
			{
				Debug.LogError("Target GameObject is not assigned in the SceneManager!");
				return;
			}

			// Get the screen bounds in world units
			Vector3 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
			Vector3 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

			var targetScript = target.GetComponent<ShipBase>();
			if (targetScript == null)
			{
				Debug.LogError("The target GameObject does not have a ShipBase component!");
				return;
			}

			for (int i = 0; i < TURRET_COUNT; i++)
			{
				// Generate a random position within the screen bounds
				float randomX = UnityEngine.Random.Range(screenBottomLeft.x, screenTopRight.x);
				float randomY = UnityEngine.Random.Range(screenBottomLeft.y, screenTopRight.y);

				var turretObject = Instantiate(_turretPrefab);
				var turret = turretObject.GetComponent<Turret>();

				if (turret != null)
				{
					turret.Initialize(targetScript);
				}

				Debug.Log($"Subscribing turret {i + 1} to PositionUpdated event.");

				turretObject.transform.position = new Vector3(randomX, randomY, 0);

				Debug.Log($"Spawned turret {i + 1} at position: {turret.transform.position}");
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