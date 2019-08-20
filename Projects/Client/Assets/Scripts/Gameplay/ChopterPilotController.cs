﻿//Rambo Team
using UnityEngine;

namespace RamboTeam.Client
{
	public class ChopterPilotController : MonoBehaviorBase
	{
		public static ChopterPilotController Instance
		{
			get;
			private set;
		}

		private const float SYNC_RATE = 2;
		private const float SYNC_PERIOD = 1 / SYNC_RATE;

		private float nextSyncTime = 0.0F;
		private Vector3 lastPosition = Vector3.zero;
		private Quaternion lastRotation = Quaternion.identity;

		private float verticalRoation = 0;
		private float horizontalRoation = 0;

		public float MovementSpeed = 10;
		public float RotationSpeed = 10;
		public float VerticalRotation = 15;
		public float HorizontalRotation = 10;

		[SerializeField]
		private GameObject ChopterModel;

		public bool IsPilot
		{
			get;
			private set;
		}

		public bool IsMoving
		{
			get;
			private set;
		}

		protected override void Awake()
		{
			base.Awake();

			Instance = this;

			lastPosition = transform.position;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			NetworkCommands.OnPilot += OnPilot;
			NetworkCommands.OnCommando += OnCommando;
			NetworkCommands.OnSyncChopterTransform += OnSyncChopterTransform;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			NetworkCommands.OnPilot -= OnPilot;
			NetworkCommands.OnCommando -= OnCommando;
			NetworkCommands.OnSyncChopterTransform -= OnSyncChopterTransform;
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

			float t = Time.deltaTime;

			bool isControlDown = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
			if (isControlDown)
			{
				Vector3 chopterForward = CameraController.Instance.transform.forward;
				chopterForward.y = 0;
				Vector3 chopterRight = CameraController.Instance.transform.right;
				chopterRight.y = 0;

				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				{
					CameraController.Instance.PanOffset += chopterForward;
				}
				if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				{
					CameraController.Instance.PanOffset -= chopterForward;
				}

				if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				{
					CameraController.Instance.PanOffset += chopterRight;
				}
				if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				{
					CameraController.Instance.PanOffset -= chopterRight;
				}
			}

			if (IsPilot)
			{
				IsMoving = false;

				verticalRoation = 0;
				horizontalRoation = 0;

				if (!isControlDown)
				{
					if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
					{
						verticalRoation = VerticalRotation;

						transform.Translate(transform.forward * Time.deltaTime * MovementSpeed, Space.World);
						IsMoving = true;
					}
					if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
					{
						verticalRoation = VerticalRotation * -1;

						transform.Translate(transform.forward * Time.deltaTime * MovementSpeed * -1, Space.World);
						IsMoving = true;
					}

					if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
					{
						horizontalRoation = HorizontalRotation;

						transform.Rotate(0, Time.deltaTime * RotationSpeed * -1, 0, Space.World);
					}
					if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
					{
						horizontalRoation = HorizontalRotation * -1;

						transform.Rotate(0, Time.deltaTime * RotationSpeed, 0, Space.World);
					}
				}

				ChopterModel.transform.localRotation = Quaternion.Lerp(ChopterModel.transform.localRotation, Quaternion.Euler(verticalRoation, 0, horizontalRoation), t);

				if (Time.time >= nextSyncTime)
				{
					NetworkCommands.SyncChopterTransform(transform.position, transform.rotation.eulerAngles);

					nextSyncTime = Time.time + SYNC_PERIOD;
				}
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, lastPosition, t);
				transform.rotation = Quaternion.Lerp(transform.rotation, lastRotation, t);

				IsMoving = ((lastPosition - transform.position).sqrMagnitude > 1);
			}
		}
	}
}