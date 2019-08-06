//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class TestObject : MonoBehaviorBase
	{
		private float nextSyncTime = 0.0F;

		protected override void Start()
		{
			base.Start();

			nextSyncTime = Time.time + 1;
		}

		protected override void Update()
		{
			base.Update();

			if (Time.time <= nextSyncTime)
			{
				SendPosition();

				nextSyncTime += 1;
			}
		}
	}
}