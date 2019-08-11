//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class CameraController : MonoBehaviorBase
	{
		public float Speed = 10.0F;
		public float BaseDistance = 30;
		public float OffsetRadius = 10;

		private ChopterPilotController chopter = null;
		private Transform chopterTransform = null;

		protected override void Start()
		{
			base.Start();

			chopter = ChopterPilotController.Instance;
			chopterTransform = chopter.transform;
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();

			Vector3 forward = chopterTransform.position + new Vector3(0, 1, -1);
			forward = (forward - chopterTransform.position).normalized;
			Vector3 targetPos = chopterTransform.position + (forward * BaseDistance);

			if (chopter.IsMoving)
			{
				Vector3 chopterForward = chopterTransform.forward;
				chopterForward.y = 0;
				float angle = Vector3.Angle(chopterForward, Vector3.right);

				if (chopterForward.z < 0)
					angle = 360 - angle;

				angle *= Mathf.Deg2Rad;

				float cosAngle = Mathf.Cos(angle);

				targetPos.x += OffsetRadius * cosAngle * (Screen.width / (float)Screen.height);
				targetPos.z += OffsetRadius * Mathf.Sin(angle);
			}

			float t = Time.deltaTime * Speed;

			transform.position = Vector3.Lerp(transform.position, targetPos, t);
			transform.forward = Vector3.Lerp(transform.forward, forward * -1, t);
		}
	}
}