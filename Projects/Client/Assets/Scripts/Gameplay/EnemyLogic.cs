//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class EnemyLogic : MonoBehaviorBase
	{
		private Transform target = null;

		private float sqrRange = 0;
		private float rateOfShot = 0;
		private float nextShotTime = 0;

		public float Range = 10;
		public float ShotPerSecond = 1;

		[SerializeField]
		private GameObject BulletPrefab = null;

		protected override void Start()
		{
			base.Start();

			sqrRange = Range * Range;
			rateOfShot = 1 / ShotPerSecond;

			target = ChopterPilotController.Instance.transform;
		}

		protected override void Update()
		{
			base.Update();

			if (target == null)
				return;

			Vector3 diff = target.position - transform.position;

			if (diff.sqrMagnitude > sqrRange)
				return;

			if (Time.time < nextShotTime)
				return;

			nextShotTime = Time.time + rateOfShot;

			Shot(diff.normalized, diff.magnitude);
		}

		protected void Shot(Vector3 Direction, float Distance)
		{
			GameObject newObject = GameObject.Instantiate(BulletPrefab);
			newObject.transform.position = transform.position;

			Bullet bullet = newObject.GetComponent<Bullet>();

			bullet.SetParamaeters(Direction);
		}

		protected override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();

			Gizmos.DrawWireSphere(transform.position, Range);
		}
	}
}