﻿//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class MissileLauncherEnemy : Enemy
	{
		protected override void OnEnable()
		{
			base.OnEnable();

			if (!IsPilot)
				NetworkCommands.OnSyncMissleLauncherShot += OnFire;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (!IsPilot)
				NetworkCommands.OnSyncMissleLauncherShot -= OnFire;
		}

		private void OnFire(Vector3 Position, Vector3 Direction)
		{
			ShotInternal(Position, Direction);
		}

		protected override void Shot(Vector3 Position, Vector3 Direction)
		{
			base.Shot(Position, Direction);

			NetworkCommands.SyncMissleLauncherShot(Position, Direction);
		}
	}
}