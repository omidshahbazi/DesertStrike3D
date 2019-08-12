//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public abstract class EnemyLogic : MonoBehaviorBase
	{
		private float sqrRange = 0;
		private float rateOfShot = 0;
		private float nextShotTime = 0;

		public float Range = 10;
		public float ShotPerSecond = 1;

		protected Transform Target
		{
			get;
			private set;
		}

		protected override void Start()
		{
			base.Start();

			sqrRange = Range * Range;
			rateOfShot = 1 / ShotPerSecond;

			Target = ChopterPilotController.Instance.transform;
		}

		protected override void Update()
		{
			base.Update();

			Vector3 diff = Target.position - transform.position;

			Target = null;

			if (diff.sqrMagnitude > sqrRange)
				return;

			if (Time.time < nextShotTime)
				return;

			nextShotTime = Time.time + rateOfShot;

			Shot(diff.normalized, diff.magnitude);
		}

		protected abstract void Shot(Vector3 Direction, float Distance);

		protected override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();

			Gizmos.DrawWireSphere(transform.position, Range);
		}
	}
}