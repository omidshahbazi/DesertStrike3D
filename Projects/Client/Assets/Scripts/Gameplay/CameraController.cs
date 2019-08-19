﻿//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class CameraController : MonoBehaviorBase
	{
		public static CameraController Instance
		{
			get;
			private set;
		}

		public float Speed = 10.0F;
		public float BaseDistance = 30;
		public float OffsetRadius = 10;

		private ChopterPilotController chopter = null;
		private Transform chopterTransform = null;

		public Vector3 PanOffset
		{
			get;
			set;
		}

		protected override void Awake()
		{
			base.Awake();

			Instance = this;
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

			Vector3 forward = chopterTransform.position + new Vector3(-1, 1, -1);
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
		}
	}
}