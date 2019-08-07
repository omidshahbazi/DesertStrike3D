//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class ChopterPilotController : MonoBehaviorBase
	{
		private float nextSyncTime = 0.0F;

		public float Speed = 10;

		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			base.Update();

			transform.localRotation = Quaternion.Euler(0, GetAngle() - 90, 0);

			if (Input.GetKey(KeyCode.Space))
				transform.Translate(transform.forward * Time.deltaTime * Speed, Space.World);

			if (Time.time <= nextSyncTime)
			{
				SendPosition();

				nextSyncTime += 1;
			}
		}

		private static float GetAngle()
		{
			Vector2 dirFromCenter = (Vector2)Input.mousePosition - new Vector2(Screen.width * 0.5F, Screen.height * 0.5F);

			float angle = Vector2.Angle(Vector2.right, dirFromCenter);

			if (dirFromCenter.y < 0)
				angle = 360 - angle;

			return 360 - angle;
		}
	}
}