﻿//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class CoPilotCameraController : MonoBehaviorBase
	{
		public static CoPilotCameraController Instance
		{
			get;
			private set;
		}

		public float MaxYawAngle = 90.0F;
		public float MaxPitchAngle = 45.0F;

		protected override void Awake()
		{
			base.Awake();

			Instance = this;
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();

			if (NetworkLayer.Instance.IsPilot)
				return;

			Vector2 screenSize = new Vector2(Screen.width / 2.0F, Screen.height / 2.0F);
			Vector2 halfScreenSize = screenSize * 0.5F;
			Vector2 mousePosition = (Vector2)Input.mousePosition;
			mousePosition.x = Mathf.Clamp(mousePosition.x, 0, screenSize.x);
			mousePosition.y = Mathf.Clamp(mousePosition.y, 0, screenSize.y);
			Vector2 normalizedMousePosition = (mousePosition - halfScreenSize) / halfScreenSize;

			float yawAngle = (MaxYawAngle / 2) * normalizedMousePosition.x;
			float pitchAngle = (MaxPitchAngle / 2) * normalizedMousePosition.y * -1;

			transform.localRotation = Quaternion.Euler(pitchAngle, yawAngle, 0);
		}
	}
}