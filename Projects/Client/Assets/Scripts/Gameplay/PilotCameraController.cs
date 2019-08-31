//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class PilotCameraController : MonoBehaviorBase
	{
		public static PilotCameraController Instance
		{
			get;
			private set;
		}

		public float Speed = 10.0F;
		public float BaseDistance = 30;
		public float OffsetRadius = 10;

		private ChopterPilotController chopter = null;
		private Transform chopterTransform = null;
		private Camera tpsCamera = null;
		private Camera fpsCamera = null;

		private bool isFPS = false;

		public Vector3 PanOffset
		{
			get;
			set;
		}

		protected override void Awake()
		{
			base.Awake();

			Instance = this;

			tpsCamera = GetComponent<Camera>();

			Transform fpsCameraObject = ChopterPilotController.Instance.transform.Find("FPSCamera");
			if (fpsCameraObject != null)
			{
				fpsCamera = fpsCameraObject.GetComponent<Camera>();
				fpsCameraObject.gameObject.SetActive(false);
			}
		}

		protected override void Start()
		{
			base.Start();

			chopter = ChopterPilotController.Instance;
			chopterTransform = chopter.transform;
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();

			Vector3 forward = chopterTransform.position + new Vector3(-1, 2, -1);
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

				targetPos.x += OffsetRadius * Mathf.Cos(angle) * (Screen.width / (float)Screen.height);
				targetPos.z += OffsetRadius * Mathf.Sin(angle);
			}

			float t = Time.deltaTime * Speed;

			transform.position = Vector3.Lerp(transform.position, targetPos + PanOffset, t);
			transform.forward = Vector3.Lerp(transform.forward, forward * -1, t);

			if (Input.GetKeyUp(KeyCode.V))
			{
				isFPS = !isFPS;

				fpsCamera.gameObject.SetActive(isFPS);
			}
		}
	}
}