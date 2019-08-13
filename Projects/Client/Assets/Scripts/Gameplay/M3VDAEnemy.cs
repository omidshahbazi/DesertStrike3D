//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class M3VDAEnemy : Enemy
	{
		protected override void OnEnable()
		{
			base.OnEnable();

			if (!IsPilot)
				NetworkCommands.OnSyncM3VDAShot += OnFire;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (!IsPilot)
				NetworkCommands.OnSyncM3VDAShot -= OnFire;
		}

		private void OnFire(Vector3 Position, Vector3 Direction)
		{
			ShotInternal(Position, Direction);
		}

		protected override void Shot(Vector3 Position, Vector3 Direction)
		{
			base.Shot(Position, Direction);

			NetworkCommands.SyncM3VDAShot(Position, Direction);
		}
	}
}