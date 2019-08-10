//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class CameraController : MonoBehaviorBase
	{
		public float Speed = 10.0F;

		private Transform targetTransform;
		private Transform chopterTransform;

		protected override void Start()
		{
			base.Start();

			targetTransform = ChopterPilotController.Instance.CameraTargetTransform;
			chopterTransform = ChopterPilotController.Instance.transform;
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();

			float t = Time.deltaTime * Speed;

			transform.position = Vector3.Lerp(transform.position, targetTransform.position, t);

			//Vector3 rot = transform.rotation.eulerAngles;
			//transform.rotation = Quaternion.Euler(rot.x, Mathf.Lerp(rot.y,  chopterTransform.rotation.y, t), rot.z);

			transform.LookAt(chopterTransform);
		}
	}
}