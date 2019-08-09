//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class ChopterPilotController : MonoBehaviorBase
	{
		private const float SYNC_PERIOD = 1.0F;

		private float nextSyncTime = 0.0F;
		private Vector3 lastPosition = Vector3.zero;
		private Quaternion lastRotation = Quaternion.identity;

		public float Speed = 10;

		public bool IsPilot
		{
			get;
			private set;
		}

		protected override void Start()
		{
			base.Start();

			NetworkCommands.OnPilot += OnPilot;
			NetworkCommands.OnCommando += OnCommando;
			NetworkCommands.OnSyncChopterTransform += OnSyncChopterTransform;
		}

		private void OnPilot()
		{
			IsPilot = true;
		}

		private void OnCommando()
		{
			IsPilot = false;
		}

		private void OnSyncChopterTransform(Vector3 Position, Vector3 Rotation)
		{
			if (IsPilot)
				return;

			lastPosition = Position;
			lastRotation = Quaternion.Euler(Rotation);

			nextSyncTime = Time.time + SYNC_PERIOD;
		}

		protected override void Update()
		{
			base.Update();

			if (IsPilot)
			{
				transform.localRotation = Quaternion.Euler(0, GetAngle() - 90, 0);

				if (Input.GetKey(KeyCode.Space))
					transform.Translate(transform.forward * Time.deltaTime * Speed, Space.World);

				if (Time.time >= nextSyncTime)
				{
					NetworkCommands.SyncChopterTransform(transform.position, transform.rotation.eulerAngles);

					nextSyncTime = Time.time + SYNC_PERIOD;
				}
			}
			else
			{
				float t = SYNC_PERIOD - (nextSyncTime - Time.time);

				transform.position = Vector3.Lerp(transform.position, lastPosition, t);
				transform.rotation = Quaternion.Lerp(transform.rotation, lastRotation, t);
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